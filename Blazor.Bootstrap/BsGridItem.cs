namespace Du.Blazor.Bootstrap;

public abstract class BsGridItem : ComponentFragment
{
	[Parameter] public BsVariant? Fore { get; set; }
	[Parameter] public BsVariant? Back { get; set; }

	/// <inheritdoc />
	protected override void OnComponentClass(CssCompose cssc)
	{
		cssc.Add(Fore is not null, (((BsVariant)Fore!)).ToCss("text"))
			.Add(Back is not null, ((BsVariant)Back!).ToCss("bg"));
	}
}


/// <summary>Grid Row</summary>
public class BsRow : BsGridItem
{
	/// <inheritdoc />
	protected override void OnComponentClass(CssCompose cssc)
	{
		cssc.Add("row");
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
		=> ComponentRenderer.TagFragment(this, builder);
}


public abstract class BsColBase : BsGridItem
{
	[Parameter] public string? Sm { get; set; }
	[Parameter] public string? Md { get; set; }
	[Parameter] public string? Lg { get; set; }
	[Parameter] public string? Xl { get; set; }
	[Parameter] public string? Xxl { get; set; }

	/// <inheritdoc />
	protected override void OnComponentClass(CssCompose cssc)
	{
		cssc.Add(Sm is not null, SizeToString("sm", Sm))
			.Add(Md is not null, SizeToString("md", Md))
			.Add(Lg is not null, SizeToString("lg", Lg))
			.Add(Xl is not null, SizeToString("xl", Xl))
			.Add(Xxl is not null, SizeToString("xxl", Xxl));

		base.OnComponentClass(cssc);
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
		=> ComponentRenderer.TagFragment(this, builder);

	//
	private static string SizeToString(string expand, string? count) =>
		$"col-{expand}-{count}";
}


/// <summary>Grid Column</summary>
public class BsCol : BsColBase
{
	[Parameter] public string? Count { get; set; }

	/// <inheritdoc />
	protected override void OnComponentClass(CssCompose cssc)
	{
		cssc.Add(Count is not null ? Count.Length == 0 ? "col" : $"col-{Count}" : null);

		base.OnComponentClass(cssc);
	}
}

public class BsColAuto : BsColBase
{
	private readonly string _col;

	//
	public BsColAuto() =>
		_col = "auto";

	//
	protected BsColAuto(int col) =>
		_col = col.ToString();

	/// <inheritdoc />
	protected override void OnComponentClass(CssCompose cssc)
	{
		cssc.Add("col-" + _col);

		base.OnComponentClass(cssc);
	}
}

/// <inheritdoc/>
public class BsCol1 : BsColAuto { public BsCol1() : base(1) { } }

/// <inheritdoc/>
public class BsCol2 : BsColAuto { public BsCol2() : base(2) { } }

/// <inheritdoc/>
public class BsCol3 : BsColAuto { public BsCol3() : base(3) { } }

/// <inheritdoc/>
public class BsCol4 : BsColAuto { public BsCol4() : base(4) { } }

/// <inheritdoc/>
public class BsCol5 : BsColAuto { public BsCol5() : base(5) { } }

/// <inheritdoc/>
public class BsCol6 : BsColAuto { public BsCol6() : base(6) { } }

/// <inheritdoc/>
public class BsCol7 : BsColAuto { public BsCol7() : base(7) { } }

/// <inheritdoc/>
public class BsCol8 : BsColAuto { public BsCol8() : base(8) { } }

/// <inheritdoc/>
public class BsCol9 : BsColAuto { public BsCol9() : base(9) { } }

/// <inheritdoc/>
public class BsCol10 : BsColAuto { public BsCol10() : base(10) { } }

/// <inheritdoc/>
public class BsCol11 : BsColAuto { public BsCol11() : base(11) { } }

/// <inheritdoc/>
public class BsCol12 : BsColAuto { public BsCol12() : base(12) { } }
