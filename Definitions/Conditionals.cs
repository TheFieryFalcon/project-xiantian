using System.Linq.Expressions;
using System.Reflection;

namespace ProjectXiantian.Definitions {
    //NOTE: if you are comparing against a constant, it MUST go on the RIGHT of the expression
    // Returns: 0 - false, 1 - true, 2 - property not found, -1 - statement or context is null
    public class ConditionalStatement(string l, string op, string r) {
        public string l { get; set; } = l;
        public string op { get; set; } = op;
        public string r { get; set; } = r;
        public static Tuple<int, LambdaExpression?> Prevaluate(ConditionalStatement Statement) {
            Expression l;
            Expression r = null; // I really don't get why I need this but I do
            if (Statement is null) {
                return new(-1, null);
            }
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
                Type root = Type.GetType(parsedr[0]);
                foreach (string propName in parsedr[1..]) {
                    try { prop = root.GetProperty(propName); } catch { return new(2, null); }
                    root = prop.PropertyType;
                }
                var value = prop.GetValue(prop); // this is the REAL use of var btw, please stop spamming it everywhere
                r = Expression.Constant(value, prop.PropertyType);
            }

            // then, parse l
            string[] parsedl = Statement.r.Split('.');
            PropertyInfo propl = null;
            Type rootl = Type.GetType(parsedl[0]);
            foreach (string propName in parsedl[1..]) {
                try { propl = rootl.GetProperty(propName); } catch { return new(2, null); }
                rootl = propl.PropertyType;
            }
            var valuel = propl.GetValue(propl);
            l = Expression.Constant(valuel, propl.PropertyType);

            // finally, determine what the expression is
            Expression op = Statement.op switch
            {
                ">" => Expression.GreaterThan(l, r),
                "<" => Expression.LessThan(l, r),
                "==" => Expression.Equal(l, r),
                "=" => Expression.Equal(l, r),
                "=/=" => Expression.NotEqual(l, r),
                _ => null
            };
            LambdaExpression o = Expression.Lambda(op);
            if (op is null) {
                return new(3, null);
            }
            return new(4, o);
        }
        public static int Evaluate(GameContext context, ConditionalStatement statement) {
            if (context == null) {
                return -1;
            }
            Tuple<int, LambdaExpression> rtn = Prevaluate(statement);
            if (rtn.Item2 == null) {
                return rtn.Item1;
            }
            else {
                Func<bool> eval = (Func<bool>)rtn.Item2.Compile();
                return eval.Invoke() switch { false => 0, true => 1 };
            }
        }

    }
    public class EventHandlers {
        public event EventHandler<GameContext> ItemConsumed;
        public class ItemConsumedEventArgs : EventArgs {
            public Item ItemConsumed {  get; set; }
        }
    }
}
