namespace Du.Blazor.Bootstrap;

/// <inheritdoc/>
public class Button : Nulo
{
}


/// <summary>버튼</summary>
public class Nulo : NuloBase
{
	#region 기본 설정
	/// <summary>버튼 설정</summary>
	public class Settings
	{
		public BsButtonType Type { get; set; }
		public BsVariant Variant { get; set; }
		public BsSize Size { get; set; }
		public bool Outline { get; set; }
	}

	/// <summary>버튼 기본값</summary>
	public static Settings DefaultSettings { get; }

	static Nulo()
	{
		DefaultSettings = new Settings
		{
			Type = BsButtonType.Button,
			Variant = BsVariant.Primary,
			Size = BsSize.Medium,
			Outline = false
		};
	}
	#endregion

	/// <summary>리스트 에이전시, 이게 있으면 리스트 메뉴용으로 처리함</summary>
	[CascadingParameter] public ITagListAgent? ListAgency { get; set; }

	/// <summary>URL 링크 지정.</summary>
	[Parameter] public string? Link { get; set; }
	/// <summary>타겟 지정.</summary>
	[Parameter] public string? Target { get; set; }

	/// <summary>리스트 에이전시가 있을 때 리스트(li)의 CSS클래스</summary>
	[Parameter] public string? ListClass { get; set; }

	//
	protected override void OnComponentClass(CssCompose cssc)
	{
		if (ListAgency is null)
			cssc.Add("btn")
				.Add(ActualVariant.ToButtonCss(ActualOutline));
		else
			cssc.Add(ListAgency.Class);

		cssc.Add(ActualSize.ToCss("btn"));
	}

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		if (ListAgency?.Tag != null)
			InternalRenderTreeListedButton(builder, ListAgency.Tag);
		else
			InternalRenderTreeButton(builder);
	}

	private void InternalRenderTreeListedButton(RenderTreeBuilder builder, string tag)
	{
		/*
		 * <li class="@ListClass">
		 *     @if (Href.IsHave())
		 *     {
		 *         <a class="@CssClass" href="@Href" target="@Target" id="@Id" @attributes="UserAttrs">
		 *             @Text
		 *             @ChildContent
		 *         </a>
		 *     }
		 *     else
		 *     {
		 *         <a role="button" class="@CssClass" id="@Id" @attributes="UserAttrs"
		 *            @onclick="HandleOnClickAsync">
		 *            @Text
		 *             @ChildContent
		 *         </a>
		 *     }
		 * </li>
		 */
		builder.OpenElement(0, tag); // li
		builder.AddAttribute(1, "class", ListClass);

		builder.OpenElement(2, "a"); // a
		builder.AddAttribute(3, "class", CssClass);

		if (Link.IsHave())
		{
			builder.AddAttribute(4, "href", Link);
			builder.AddAttribute(5, "target", Target);
		}
		else
		{
			builder.AddAttribute(6, "role", "button");
			builder.AddAttribute(7, "onclick", HandleOnClickAsync);
		}

		builder.AddAttribute(8, "id", Id);
		builder.AddMultipleAttributes(9, UserAttrs);
		if (Text.IsHave())
			builder.AddContent(10, Text);
		builder.AddContent(11, ChildContent);
		builder.CloseElement(); // a

		builder.CloseElement(); // li
	}

	private void InternalRenderTreeButton(RenderTreeBuilder builder)
	{
		/*
		 * if (Href.IsHave())
		 * {
		 *     <a class="@CssClass" href="@Href" target="@Target" id="@Id" @attributes="UserAttrs">
		 *         @Text
		 *         @ChildContent
		 *     </a>
		 * }
		 * else
		 * {
		 *     <bsButton type="@Actual.ToHtml()" class="@CssClass" formtarget="@Target" id="@Id" 		
		 *        @attributes="UserAttrs" @onclick="HandleOnClickAsync">
		 *         @Text
		 *         @ChildContent
		 *     </bsButton>
		 *     }
		 */
		var link = Link.IsHave();

		builder.OpenElement(0, link ? "a" : "button"); // a or bsButton
		builder.AddAttribute(1, "class", CssClass);

		if (link)
		{
			builder.AddAttribute(2, "href", Link);
			builder.AddAttribute(3, "target", Target);
		}
		else
		{
			builder.AddAttribute(4, "type", Actual.ToHtml());
			builder.AddAttribute(5, "formtarget", Target);
			builder.AddAttribute(6, "onclick", HandleOnClickAsync);
		}

		builder.AddAttribute(7, "id", Id);
		builder.AddMultipleAttributes(8, UserAttrs);
		if (Text.IsHave())
			builder.AddContent(9, Text);
		builder.AddContent(10, ChildContent);
		builder.CloseElement(); // a or bsButton
	}
}


/// <summary>버튼 베이스</summary>
/// <seealso cref="ComponentFragment" />
public abstract class NuloBase : ComponentFragment
{
	/// <summary>에디트 컨텍스트</summary>
	[CascadingParameter] public EditContext? EditContext { get; set; }

	/// <summary>텍스트</summary>
	[Parameter] public string? Text { get; set; }
	/// <summary>버튼 타입. <see cref="BsButtonType" /> 참고</summary>
	[Parameter] public BsButtonType? Type { get; set; }
	/// <summary>레이아웃 타입. <see cref="BsVariant" /> 참고</summary>
	[Parameter] public BsVariant? Variant { get; set; }
	/// <summary>컴포넌트 크기. <see cref="BsSize" /> 참고</summary>
	[Parameter] public BsSize? Size { get; set; }
	/// <summary>아웃라인 적용.</summary>
	[Parameter] public bool? Outline { get; set; }

	/// <summary>마우스 눌린 이벤트 지정.</summary>
	[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
	/// <summary>에디트 폼 ValidClick.</summary>
	[Parameter] public EventCallback<MouseEventArgs> OnValidClick { get; set; }
	/// <summary>에디트 폼 InvalidClick.</summary>
	[Parameter] public EventCallback<MouseEventArgs> OnInvalidClick { get; set; }

	//
	protected BsButtonType Actual => Type ?? Nulo.DefaultSettings.Type;
	protected BsVariant ActualVariant => Variant ?? Nulo.DefaultSettings.Variant;
	protected BsSize ActualSize => Size ?? Nulo.DefaultSettings.Size;
	protected bool ActualOutline => Outline ?? Nulo.DefaultSettings.Outline;

	//
	private bool _handle_click;

	// OnComponentInitialized를 안쓰고 이걸 쓴 이유는... 베이스 컴포넌트니깐!
	protected override void OnInitialized()
	{
		Type ??= EditContext is null ? BsButtonType.Button : BsButtonType.Submit;

		base.OnInitialized();
	}

	// 마우스 핸들러
	protected async Task HandleOnClickAsync(MouseEventArgs e)
	{
		if (!_handle_click)
		{
			_handle_click = true;

			if (OnClick.HasDelegate)
				await InvokeOnClickAsync(e);
			else if (Type == BsButtonType.Submit && EditContext != null)
				switch (EditContext.Validate())
				{
					case true when OnValidClick.HasDelegate:
						await InvokeOnValidClickAsync(e);
						break;

					case false when OnInvalidClick.HasDelegate:
						await InvokeOnInvalidClickAsync(e);
						break;
				}

			_handle_click = false;
		}
	}

	//
	protected virtual Task InvokeOnClickAsync(MouseEventArgs e) => OnClick.InvokeAsync(e);
	protected virtual Task InvokeOnValidClickAsync(MouseEventArgs e) => OnValidClick.InvokeAsync(e);
	protected virtual Task InvokeOnInvalidClickAsync(MouseEventArgs e) => OnInvalidClick.InvokeAsync(e);
}
