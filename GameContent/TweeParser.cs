using BidirectionalMap;
using ProjectXiantian.Definitions;
using System.Text;
public enum NodeType {
    NULL,
    STORY,
    LOCATION,
    BATTLE
}
public class TweeNode(Tuple<string, int, int> Address, string AccessionStatement, NodeType Type, string Content, List<string> Properties) // Node = Passage
{
    public Tuple<string, int, int> Address { get; set; } = Address;
    public List<Tuple<TweeNode, string, ConditionalStatement>>? NextNodes { get; set; } = new();
    public List<TweeNode> PreviousNodes { get; set; } = new();
    public string AccessionStatement { get; set; } = AccessionStatement;
    public ConditionalStatement AccessionCondition {  get; set; }
    public string? Content { get; set; } = Content;
    public List<Tuple<string, int, int>> ProtoAddresses { get; set; } = new();
    public NodeType Type { get; set; } = Type;
    public List<string> Properties = Properties;
    public bool Accessed = false; 
}
public class TweeTree(TweeNode root) {
    public TweeNode Root { get; set; } = root;
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
            foreach (Tuple<TweeNode, string, ConditionalStatement> node in current.NextNodes) {
                TweeNode? found = RTraverse(node.Item1, target, log);
                if (found != null) { return found; }
            }
        }
        return null;
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
        string startnode = null;
		using (var sr = new StreamReader(Misc.StoryTreePath, System.Text.Encoding.UTF8))
		{
			while (true) {
				string line = sr.ReadLine();
				if (line == null) {
					break;
				}
				if (line != "" && line.Length > 3) {
                    if (line[..3] == ":: ") {
                        string name = line[3..].Split(" ")[0];
                        switch (name) {
                            case "StoryTitle":
                                break;
                            case "StoryData":
                                while (true) {
                                    string l = sr.ReadLine().Trim();
                                    if (l != null && l.Length > 8 && l[..8] == "\"start\":") {
                                        startnode = l[10..^2];
                                        break;
                                    }
                                }
                                break;
                            default:
                                if (name == startnode) {
                                    string content = "";
                                    contentover = false;
                                    TweeNode i = null;
                                    while (true) {
                                        string l = sr.ReadLine().Trim(' ');
                                        if (l != "@" && contentover == false) { content += l; }
                                        else if (contentover == true) {
                                            if (l != "" && l != "@") {
                                                string[] components = l[2..].Split("-");
                                                i.ProtoAddresses.Add(new Tuple<string, int, int>(components[0], int.Parse(components[1]), int.Parse(components[2][..2])));
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
                                            i = new TweeNode(new Tuple<string, int, int>("START", 0, 0), null, NodeType.STORY, content, ["start", "saveable"]);
                                        }
                                    }
                                    break;
                                }
                                else {
                                    string content = "";
                                    int j = 0;
                                    string accession = null;
                                    string accessioncondition = null;
                                    NodeType type = NodeType.NULL;
                                    List<string> properties = new();
                                    contentover = false;
                                    Tuple<string, int, int> Address = null;
                                    TweeNode i = null;
                                    while (true) {
                                        string l = sr.ReadLine();
                                        Console.WriteLine(l);
                                        if (l != null) {
                                            l = l.Trim(' ');
                                        }
                                        // This triggers first
                                        if (j == 0) {
                                            accession = l;
                                        }
                                        // This triggers second
                                        else if (j == 1) {
                                            // if parse did not work, then l is not a valid type, so it must be an accession condition
                                            // TODO: actually implement parsing of accession conditions (all we should need are comparisons)
                                            if (Enum.TryParse(l.Split(", ")[0], out type) == false) {
                                                accessioncondition = l;
                                                l = sr.ReadLine();
                                                type = Enum.Parse<NodeType>(l.Split(", ")[0]);
                                                foreach (string Property in l.Split(", ")[1..]) {
                                                    properties.Add(Property);
                                                }
                                                j++;
                                            }
                                            else {
                                                type = Enum.Parse<NodeType>(l.Split(", ")[0]);
                                                foreach (string Property in l.Split(", ")[1..]) {
                                                    properties.Add(Property);
                                                }
                                            }
                                        }
                                        // This triggers third
                                        else if (l != "@" && contentover == false) { content = content + "\n" + l; }
                                        // This triggers fifth
                                        else if (contentover == true) {
                                            if (l != "" && l != "@" && l != null && l != "\n" && l != "\r") {
                                                string[] components = l[2..^2].Split("-");
                                                i.ProtoAddresses.Add(new Tuple<string, int, int>(components[0], int.Parse(components[1]), int.Parse(components[2][..2])));
                                            }
                                            else if (l == "@") { }
                                            // This should always trigger last
                                            else {
                                                NodeSet.Add(i, Address);
                                                break;
                                            }
                                        }
                                        // This triggers fourth
                                        else {
                                            contentover = true;
                                            string[] NameComponents = name.Split("-");
                                            Address = new(NameComponents[0], int.Parse(NameComponents[1]), int.Parse(NameComponents[2]));

                                            i = new TweeNode(Address, accession, type, content, properties);
                                            if (accessioncondition != null) {
                                                string[] parts = accessioncondition.Split(" ");
                                                i.AccessionCondition = new(parts[0], parts[1], parts[2]);
                                            }
                                        }
                                        j++;
                                    }
                                    break;
                                }
                            }
                        }
                    }
				}

			}
            // build tree
            TweeTree tree = new(root);
            foreach (KeyValuePair<TweeNode, Tuple<string, int, int>> node in NodeSet) {
                foreach (Tuple<string, int, int> Address in node.Key.ProtoAddresses) {
                    TweeNode next = NodeSet.Reverse[Address];
                    node.Key.NextNodes.Add(new(NodeSet.Reverse[Address], NodeSet.Reverse[Address].AccessionStatement, NodeSet.Reverse[Address].AccessionCondition));
                    next.PreviousNodes.Add(node.Key);
                }
            }

			return(tree);
		}
	}
