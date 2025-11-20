using BidirectionalMap;
public enum SectionCatalogue
{
    START = 0,
    TOWN = 14 //PLACEHOLDER
}

public class TweeNode // Node = Passage
{
    public Tuple<string, int, int> Address { get; set; } // addresses take the form SECTION-Stem-Leaf
    public List<TweeNode>? NextNodes { get; set; } = new();
    public TweeNode? PreviousNode { get; set; }
    public string AccessionStatement { get; set; }
    public string? Content { get; set; }

    public List<Tuple<string, int, int>> ProtoAddresses { get; set; } = new();

    public TweeNode(Tuple<string, int, int> Address, string AccessionStatement, string Content)
    {
        this.Address = Address;
        this.AccessionStatement = AccessionStatement;
        this.Content = Content;
    }
}
public class TweeTree
{
    public TweeNode Root { get; set; }
    public TweeNode? Traverse(Tuple<string, int, int> target) //Depth-first recursive traversal algorithm, pretty standard fare I'd say
    {
        if (Root == null) {return null;}
        HashSet<TweeNode> Log = new HashSet<TweeNode>();
        return RTraverse(Root, target, Log);
    }
    private TweeNode? RTraverse(TweeNode current, Tuple<string, int, int> target, HashSet<TweeNode> log) {
        if (current == null || log.Contains(current)) { return null; }
        log.Add(current);
        if (current.Address.Item1 == target.Item1 && current.Address.Item2 == target.Item2 && current.Address.Item3 == target.Item3) { return current; }
        if (current.NextNodes != null) {
            foreach (TweeNode node in current.NextNodes) {
                TweeNode? found = RTraverse(node, target, log);
                if (found != null) { return found; }
            }
        }
        return null;
    }

    public TweeTree(TweeNode root)
    {
        Root = root;
    }


}
public class TweeParser
{
	public static TweeTree Parse()
	{
        // Build nodes
        BiMap<TweeNode, Tuple<string, int, int>> NodeSet = new();
        TweeNode root = null;
        bool contentover;
		using (var sr = new StreamReader("ProjectXiantian.twee", System.Text.Encoding.UTF8))
		{
			while (true) {
				string line = sr.ReadLine();
				if (line == null) {
					break;
				}
				if (line != "" && line.Length > 3) {
                    if (line.Remove(3) == ":: ") {
                        string name = line.Substring(3).Split(" ")[0];
                        switch (name) {
                            case "StoryTitle":
                                break;
                            case "StoryData":
                                break;
                            case "START-0-00":
                                string content = "";
                                contentover = false;
                                TweeNode i = null;
                                while (true) {
                                    string l = sr.ReadLine();
                                    if (l != "@" && contentover == false) { content = content + l; }
                                    else if (contentover == true) {
                                        if (l != "" && l != "@") {
                                            string[] components = l.Substring(2, l.Count() - 2).Split("-");
                                            i.ProtoAddresses.Add(new Tuple<string, int, int>(components[0], int.Parse(components[1]), int.Parse(components[2].Remove(2))));
                                        }
                                        else if (l == "@") { }
                                        else {
                                            NodeSet.Add(i, new Tuple<string, int, int>("START", 0, 0));
                                            root = i;
                                            break;
                                        }
                                    }
                                    else {
                                        contentover = true;
                                        i = new TweeNode(new Tuple<string, int, int>("START", 0, 0), null, content);
                                    }
                                }

                                break;
                            default:
                                content = "";
                                int j = 0;
                                string accession = null;
                                contentover = false;
                                Tuple<string, int, int> Address = null;
                                i = null;
                                while (true) {
                                    string l = sr.ReadLine();
                                    if (j == 0) {
                                        accession = l;
                                    }
                                    else if (l != "@" && contentover == false) { content = content + l; }
                                    else if (contentover == true) {
                                        if (l != "" && l != "@" && l != null) {
                                            string[] components = l.Substring(2, l.Length - 2).Split("-");
                                            i.ProtoAddresses.Add(new Tuple<string, int, int>(components[0], int.Parse(components[1]), int.Parse(components[2].Remove(2))));
                                        }
                                        else if (l == "@") { }
                                        else {
                                            NodeSet.Add(i, Address);
                                            break;
                                        }
                                    }
                                    else {
                                        contentover = true;
                                        string[] NameComponents = name.Split("-");
                                        Address = new(NameComponents[0], int.Parse(NameComponents[1]), int.Parse(NameComponents[2]));
                                        i = new TweeNode(Address, accession, content);
                                    }
                                    j++;
                                }
                                break;
                            }
                        }
                    }
				}

			}
            // build tree
            TweeTree tree = new(root);
            foreach (KeyValuePair<TweeNode, Tuple<string, int, int>> node in NodeSet) {
                foreach (Tuple<string, int, int> Address in node.Key.ProtoAddresses) {
                    node.Key.NextNodes.Add(NodeSet.Reverse[Address]);
                    NodeSet.Reverse[Address].PreviousNode = node.Key;
                }
                node.Key.PreviousNode = NodeSet.Reverse[node.Key.Address];
            }

			return(tree);
		}
	}
