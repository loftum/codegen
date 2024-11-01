using System.Text;

namespace Example.CodeGen;

public class SourceWriter
{
    private int _indentation;
    
    private readonly StringBuilder _builder = new();

    public Indenter Indent()
    {
        IncreaseIndent();
        return new Indenter(DecreaseIndent);
    }
    
    public Indenter Block()
    {
        AppendLine("{");
        IncreaseIndent();
        return new Indenter(EndBlock);
    }

    private void EndBlock()
    {
        DecreaseIndent();
        AppendLine("}");
    }

    private void IncreaseIndent()
    {
        _indentation++;
    }

    private void DecreaseIndent()
    {
        if (_indentation > 0)
        {
            _indentation--;
        }
    }

    public SourceWriter AppendLine()
    {
        _builder.AppendLine();
        return this;
    }
    
    public SourceWriter AppendLine(string line)
    {
        var lines = line.Split('\r', '\n');
        return AppendLines(lines);
    }

    public SourceWriter AppendLines(IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            for (var ii = 0; ii < _indentation; ii++)
            {
                _builder.Append("    ");
            }
            _builder.AppendLine(line);
        }

        return this;
    }

    public override string ToString() => _builder.ToString();
}

public readonly struct Indenter(Action callback) : IDisposable
{
    public void Dispose()
    {
        callback();
    }
}