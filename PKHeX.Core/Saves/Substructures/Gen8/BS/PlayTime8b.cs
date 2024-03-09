using System;
using System.ComponentModel;

namespace PKHeX.Core;

/// <summary>
/// Playtime storage
/// </summary>
[TypeConverter(typeof(ExpandableObjectConverter))]
public sealed class PlayTime8b : PlayTime<SaveFile>
{
    public PlayTime8b(SAV8BS sav, Memory<byte> raw) : base(sav, raw) { }
    public PlayTime8b(SAV8LA sav, SCBlock block) : base(sav, block.Data) { }
}
