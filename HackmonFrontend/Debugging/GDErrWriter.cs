using System;
using System.IO;
using System.Text;
using Godot;

namespace Hackmon.Debugging
{
    public class GDErrWriter : StringWriter
    {
        public override void Write(char value)
            => GD.PushError(value);

        public override void Write(char[] buffer, int index, int count)
            => GD.PushError(new string(buffer, index, count));

        public override void Write(ReadOnlySpan<char> buffer)
            => GD.PushError(new string(buffer));

        public override void Write(string value)
            => GD.PushError(value);

        public override void Write(StringBuilder value)
            => GD.PushError(value.ToString());

        public override void WriteLine(ReadOnlySpan<char> buffer)
            => GD.PushError(new string(buffer));

        public override void WriteLine(StringBuilder value)
            => GD.PushError(value.ToString());
    }
}