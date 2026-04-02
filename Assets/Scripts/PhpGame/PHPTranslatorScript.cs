using System;
using System.Collections.Generic;

public class PHPTranslatorScript
{
    Dictionary<string, object> vars = new();
    Dictionary<string, object> post = new();

    public List<string> output = new();
    public object Result;

    // ===== POST =====
    public void SetPost(string key, object value)
    {
        post[key] = value;
    }

    // ===== EXECUTE =====
    public void Execute(string code)
    {
        var lines = code.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        ExecuteBlock(lines, 0, lines.Length);
    }

    void ExecuteBlock(string[] lines, int start, int end)
    {
        for (int i = start; i < end; i++)
        {
            var line = lines[i].Trim();

            if (string.IsNullOrEmpty(line)) continue;

            // echo
            if (line.StartsWith("echo"))
            {
                var expr = line.Substring(4).Trim().Trim(';');
                output.Add(Eval(expr)?.ToString());
            }

            // result
            else if (line.StartsWith("result"))
            {
                var expr = line.Split('=')[1].Trim().Trim(';');
                Result = Eval(expr);
            }

            // if
            else if (line.StartsWith("if"))
            {
                int blockStart = i + 1;
                int blockEnd = FindBlockEnd(lines, blockStart);

                bool condition = EvalCondition(line);

                if (condition)
                {
                    ExecuteBlock(lines, blockStart, blockEnd);
                }
                else if (blockEnd + 1 < lines.Length && lines[blockEnd + 1].Trim().StartsWith("else"))
                {
                    int elseStart = blockEnd + 2;
                    int elseEnd = FindBlockEnd(lines, elseStart);
                    ExecuteBlock(lines, elseStart, elseEnd);
                    i = elseEnd;
                    continue;
                }

                i = blockEnd;
            }

            // while
            else if (line.StartsWith("while"))
            {
                int blockStart = i + 1;
                int blockEnd = FindBlockEnd(lines, blockStart);

                while (EvalCondition(line))
                {
                    ExecuteBlock(lines, blockStart, blockEnd);
                }

                i = blockEnd;
            }

            // переменная
            else if (line.Contains("="))
            {
                var parts = line.Split('=');
                var name = parts[0].Trim().Replace("$", "");
                var value = Eval(parts[1].Trim().Trim(';'));

                vars[name] = value;
            }
        }
    }

    int FindBlockEnd(string[] lines, int start)
    {
        int depth = 0;
        for (int i = start; i < lines.Length; i++)
        {
            if (lines[i].Contains("{")) depth++;
            if (lines[i].Contains("}"))
            {
                if (depth == 0) return i - 1;
                depth--;
            }
        }
        return lines.Length - 1;
    }

    // ===== EVAL =====
    object Eval(string expr)
    {
        expr = expr.Trim();

        // $_POST
        if (expr.StartsWith("$_POST"))
        {
            var key = expr.Substring(expr.IndexOf('[') + 2);
            key = key.Substring(0, key.IndexOf('\''));

            return post.ContainsKey(key) ? post[key] : null;
        }

        // string
        if (expr.StartsWith("\"") && expr.EndsWith("\""))
            return expr.Substring(1, expr.Length - 2);

        // strlen()
        if (expr.StartsWith("strlen"))
        {
            var inside = expr.Substring(7).Trim('(', ')', ' ');
            var val = Eval(inside);

            return val != null ? val.ToString().Length : 0;
        }

        // isset
        if (expr.StartsWith("isset"))
        {
            var inside = expr.Substring(6).Trim('(', ')', ' ');

            if (inside.StartsWith("$_POST"))
            {
                var key = inside.Substring(inside.IndexOf('[') + 2);
                key = key.Substring(0, key.IndexOf('\''));
                return post.ContainsKey(key);
            }

            var name = inside.Replace("$", "");
            return vars.ContainsKey(name);
        }

        // конкатенация .
        if (expr.Contains("."))
        {
            var parts = expr.Split('.');
            string result = "";

            foreach (var part in parts)
            {
                var val = Eval(part);
                result += val?.ToString() ?? "";
            }

            return result;
        }

        // ||
        if (expr.Contains("||"))
        {
            var parts = expr.Split(new[] { "||" }, StringSplitOptions.None);
            return Convert.ToBoolean(Eval(parts[0])) || Convert.ToBoolean(Eval(parts[1]));
        }

        // &&
        if (expr.Contains("&&"))
        {
            var parts = expr.Split(new[] { "&&" }, StringSplitOptions.None);
            return Convert.ToBoolean(Eval(parts[0])) && Convert.ToBoolean(Eval(parts[1]));
        }

        // ==
        if (expr.Contains("=="))
        {
            var parts = expr.Split(new[] { "==" }, StringSplitOptions.None);
            return Equals(Eval(parts[0]), Eval(parts[1]));
        }

        // !=
        if (expr.Contains("!="))
        {
            var parts = expr.Split(new[] { "!=" }, StringSplitOptions.None);
            return !Equals(Eval(parts[0]), Eval(parts[1]));
        }

        // >
        if (expr.Contains(">"))
        {
            var parts = expr.Split('>');
            return Convert.ToInt32(Eval(parts[0])) > Convert.ToInt32(Eval(parts[1]));
        }

        // <
        if (expr.Contains("<"))
        {
            var parts = expr.Split('<');
            return Convert.ToInt32(Eval(parts[0])) < Convert.ToInt32(Eval(parts[1]));
        }

        // +
        if (expr.Contains("+"))
        {
            var parts = expr.Split('+');
            return Convert.ToInt32(Eval(parts[0])) + Convert.ToInt32(Eval(parts[1]));
        }

        // переменная
        if (expr.StartsWith("$"))
        {
            var name = expr.Replace("$", "");
            return vars.ContainsKey(name) ? vars[name] : null;
        }

        // число
        if (int.TryParse(expr, out int num))
            return num;

        return expr;
    }

    bool EvalCondition(string line)
    {
        var condition = line.Substring(line.IndexOf('(') + 1);
        condition = condition.Substring(0, condition.LastIndexOf(')'));
        return Convert.ToBoolean(Eval(condition));
    }
}