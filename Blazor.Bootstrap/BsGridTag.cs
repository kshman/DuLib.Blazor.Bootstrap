namespace Du.Blazor.Bootstrap;

/// <summary>그리드용 태그</summary>
public abstract class BsGridTag : ComponentFragment
{
	[Parameter] public BsVariant? Fore { get; set; }
	[Parameter] public BsVariant? Back { get; set; }
	[Parameter] public BsItemAlignment? Align { get; set; }

	/// <inheritdoc />
	protected override void OnComponentClass(CssCompose cssc)
	{
		cssc.Add(Fore?.ToCss("text"))
			.Add(Back?.ToCss("bg"));
	}
}


/// <summary>컨테이너. 보통 그리드용</summary>
public class BsContainer : ComponentFragment
{
	[Parameter] public BsExpand? Container { get; set; }

	/// <inheritdoc />
	protected override void OnComponentClass(CssCompose cssc)
	{
		cssc.Add(Container is null ? "container" : Container.Value.ToCss("container"));
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder) =>
		ComponentRenderer.TagFragment(this, builder);
}

/// <summary>그리드 줄</summary>
public class BsRow : BsGridTag
{
	[Parameter] public BsJustify? Justify { get; set; }

	/// <inheritdoc />
	protected override void OnComponentClass(CssCompose cssc)
	{
		cssc.Add("row")
			.Add(Align?.ToCss("align-items"))
			.Add(Justify?.ToCss("justify-content"));
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder) => ComponentRenderer.TagFragment(this, builder);
}

/// <summary>그리드 열</summary>
public abstract class BsColBase : BsGridTag
{
	[Parameter] public string? Sm { get; set; }
	[Parameter] public string? Md { get; set; }
	[Parameter] public string? Lg { get; set; }
	[Parameter] public string? Xl { get; set; }
	[Parameter] public string? Xxl { get; set; }
	[Parameter] public string? Order { get; set; }

	/// <inheritdoc />
	protected override void OnComponentClass(CssCompose cssc)
	{
		cssc.Add(SizeToString("sm", Sm))
			.Add(SizeToString("md", Md))
			.Add(SizeToString("lg", Lg))
			.Add(SizeToString("xl", Xl))
			.Add(SizeToString("xxl", Xxl))
			.Add(OrderToString(Order))
			.Add(Align?.ToCss("align-self"));

		base.OnComponentClass(cssc);
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder) => ComponentRenderer.TagFragment(this, builder);

	//
	private static string? SizeToString(string expand, string? count) =>
		count is null ? null : $"col-{expand}-{count}";

	//
	private static string? OrderToString(string? order) =>
		order is null ? null : $"order-{order}";
}

/// <inheritdoc />
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

/// <inheritdoc />
public class BsColAuto : BsColBase
{
	private readonly string _col;

	//
	public BsColAuto() => _col = "auto";

	//
	protected BsColAuto(int col) => _col = col.ToString();

	/// <inheritdoc />
	protected override void OnComponentClass(CssCompose cssc)
	{
		cssc.Add("col-" + _col);

		base.OnComponentClass(cssc);
	}
}

/// <inheritdoc/>
public class BsCol1 : BsColAuto
{
	public BsCol1() : base(1)
	{
	}
}

/// <inheritdoc/>
public class BsCol2 : BsColAuto
{
	public BsCol2() : base(2)
	{
	}
}

/// <inheritdoc/>
public class BsCol3 : BsColAuto
{
	public BsCol3() : base(3)
	{
	}
}

/// <inheritdoc/>
public class BsCol4 : BsColAuto
{
	public BsCol4() : base(4)
	{
	}
}

/// <inheritdoc/>
public class BsCol5 : BsColAuto
{
	public BsCol5() : base(5)
	{
	}
}

/// <inheritdoc/>
public class BsCol6 : BsColAuto
{
	public BsCol6() : base(6)
	{
	}
}

/// <inheritdoc/>
public class BsCol7 : BsColAuto
{
	public BsCol7() : base(7)
	{
	}
}

/// <inheritdoc/>
public class BsCol8 : BsColAuto
{
	public BsCol8() : base(8)
	{
	}
}

/// <inheritdoc/>
public class BsCol9 : BsColAuto
{
	public BsCol9() : base(9)
	{
	}
}

/// <inheritdoc/>
public class BsCol10 : BsColAuto
{
	public BsCol10() : base(10)
	{
	}
}

/// <inheritdoc/>
public class BsCol11 : BsColAuto
{
	public BsCol11() : base(11)
	{
	}
}

/// <inheritdoc/>
public class BsCol12 : BsColAuto
{
	public BsCol12() : base(12)
	{
	}
}
