using System.Text;

namespace SudokuMasterBase.Serialization;

internal static class Csv
{
    public static string Escape(string value) =>
        value.IndexOfAny([',', '"', '\r', '\n']) < 0
            ? value
            : "\"" + value.Replace("\"", "\"\"") + "\"";

    public static List<string[]> Read(TextReader reader)
    {
        var records = new List<string[]>();
        var fields = new List<string>();
        var field = new StringBuilder();
        var quoted = false;
        var closed = false;
        var started = false;
        while (reader.Read() is var next && next >= 0)
        {
            var c = (char)next;
            started = true;
            if (quoted)
            {
                if (c != '"') field.Append(c);
                else if (reader.Peek() == '"') { reader.Read(); field.Append('"'); }
                else { quoted = false; closed = true; }
                continue;
            }
            if (closed && c != ',' && c != '\r' && c != '\n')
                throw new FormatException("Unexpected character after a quoted CSV field.");
            if (c == '"')
            {
                if (field.Length != 0 || closed)
                    throw new FormatException("Quote inside an unquoted CSV field.");
                quoted = true;
            }
            else if (c == ',' || c == '\r' || c == '\n')
            {
                fields.Add(field.ToString());
                field.Clear();
                closed = false;
                if (c == ',') continue;
                if (c == '\r' && reader.Peek() == '\n') reader.Read();
                records.Add(fields.ToArray());
                fields.Clear();
                started = false;
            }
            else field.Append(c);
        }
        if (quoted) throw new FormatException("Unterminated quoted CSV field.");
        if (started)
        {
            fields.Add(field.ToString());
            records.Add(fields.ToArray());
        }
        return records;
    }
}
