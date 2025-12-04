using Baksteen.Extensions.DeepCopy;
using ProjectXiantian.GameContent.Lang;
using System.Linq.Expressions;
using System.Reflection;

namespace ProjectXiantian.Definitions {
    //NOTE: if you are comparing against a constant, it MUST go on the RIGHT of the expression
    // Returns: 0 - false, 1 - true, 2 - property not found, 3 - invalid operator, -1 - statement or context is null, 5 - two errors occured both l/r
    // This is a bit of a misnomer because it includes assignment too

    public class ConditionalStatement {
        string? l;
        string? r;
        ConditionalStatement? cl;
        ConditionalStatement? cr;
        string op;
        public ConditionalStatement(string l, string op, string r) {
            this.l = l;
            this.r = r;
            this.op = op;
        }
        public ConditionalStatement(ConditionalStatement cl, string op, ConditionalStatement cr) {
            this.cl = cl;
            this.op = op;
            this.cr = cr;
        }
        public override string ToString() {
            if (cl is not null) {
                return ($"{cl.ToString()} \n{op switch {"&&" => "and", "||" => "or"}} {cr.ToString()}"); 
            }
            string localizedl = km.lang[km.GetKey(l)];
            string localizedr = null;
            if (r != null && r.StartsWith("b") || r.StartsWith("i") || r.StartsWith("s")) {
                localizedr = r[1..];
            }
            else {
                localizedr = km.lang[km.GetKey(r)];
            }
            return ($"{localizedl} {op switch { "==" => "=", "+=" => "increases by", "-=" => "decreases by", _ => op }} {localizedr}");
        }
        public static Tuple<int, Expression?> Prevaluate(GameContext context, ConditionalStatement Statement) {
            Expression l = null;
            string lpath = null;
            Expression r = null; // I really don't get why I need this but I do
            string rpath = null;
            Expression econtext = Expression.Constant(context);
            if (Statement is null) {
                return new(-1, null);
            }
            if (Statement.l == null) {
                Tuple<int, Expression> tl = Prevaluate(context, Statement.cl);
                Tuple<int, Expression> tr = Prevaluate(context, Statement.cr);
                l = tl.Item2;
                r = tr.Item2;
                if (l is null || r is null) {
                    if (tl.Item1 == 4) {
                        return new(tl.Item1, null);
                    }
                    else if (tr.Item1 == 4) {
                        return new(tr.Item1, null);
                    }
                    else {
                        return new(5, null);
                    }
                }
            }
            else {
                // first, check if r is a constant primitive; all types are PascalCase, so lowercase means is a constant primitive. All parameters are capitalized because of this check
                if (Statement.r.StartsWith("b") || Statement.r.StartsWith("i") || Statement.r.StartsWith("s")) {
                    if (Statement.r.StartsWith("b")) {
                        r = Expression.Constant(Statement.r, typeof(bool));
                    }
                    else if (Statement.r.StartsWith("i")) {
                        r = Expression.Constant(Statement.r, typeof(int));
                    }
                    else if (Statement.r.StartsWith("s")) {
                        r = Expression.Constant(Statement.r, typeof(string));
                    }
                }
                // if not, parse r
                else {
                    string[] parsedr = Statement.r.Split('.');
                    PropertyInfo prop = null;
                    Type root = context.GetType();
                    string propName = null;
                    Expression eroot = econtext;
                    // creates an expression chain to the end property or field
                    foreach (string propname in parsedr[1..]) {
                        propName = propname; // I really don't get c# sometimes
                        rpath = string.Concat(rpath, ".", propname); // for localization
                        try { prop = root.GetProperty(propName); } catch { return new(2, null); }
                        root = prop.PropertyType;
                        eroot = Expression.PropertyOrField(eroot, propName);
                    }
                    r = Expression.PropertyOrField(eroot, propName);
                }

                // then, parse l
                string[] parsedl = Statement.l.Split('.');
                PropertyInfo propl = null;
                Type rootl = context.GetType();
                string propNamel = null;
                Expression erootl = econtext;
                foreach (string propname in parsedl[1..]) {
                    propNamel = propname;
                    lpath = string.Concat(lpath, ".", propname); // for localization
                    try { propl = rootl.GetProperty(propNamel); } catch { return new(2, null); }
                    rootl = propl.PropertyType;
                    erootl = Expression.PropertyOrField(erootl, propNamel);
                }
                r = Expression.PropertyOrField(erootl, propNamel);
            }

            // finally, determine what the expression is
            Expression op = Statement.op switch
            {
                ">" => Expression.GreaterThan(l, r),
                "<" => Expression.LessThan(l, r),
                "==" => Expression.Equal(l, r),
                "=" => Expression.Assign(l, r),
                "=/=" => Expression.NotEqual(l, r),
                "+=" => Expression.AddAssign(l, r),
                "-=" => Expression.SubtractAssign(l, r),
                "&&" => Expression.And(l, r),
                "||" => Expression.Or(l, r),
                _ => null
            };
            if (op is null) {
                return new(3, null);
            }
            return new(4, op);
        }
        public static Tuple<GameContext, int> Evaluate(GameContext context, ConditionalStatement statement) {
            if (context == null) {
                Exceptions.E2();
                return new(context, -1);
            }
            Tuple<int, Expression> rtn = Prevaluate(context, statement);
            if (rtn.Item2 == null) {
                return new(context, rtn.Item1);
            }
            // if assign expression, no need for return, just pass context to Action delegate to modify, similar to how there's no need to return a new int j in i = i + 1
            else if (rtn.Item2.NodeType == ExpressionType.Assign) {
                ParameterExpression ccontext = Expression.Parameter(typeof(GameContext));
                Action<GameContext> eval = Expression.Lambda<Action<GameContext>>(rtn.Item2, ccontext).Compile();
                return new(context, 4);
            }
            else {
                Func<bool> eval = (Func<bool>)Expression.Lambda(rtn.Item2).Compile();
                return new(context, eval.Invoke() switch { false => 0, true => 1 });
            }
        }
    }
}
