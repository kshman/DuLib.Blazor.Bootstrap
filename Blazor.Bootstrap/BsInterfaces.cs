﻿namespace Du.Blazor.Bootstrap;

/// <summary>
/// 컴포넌트 스토리지 인터페이스
/// </summary>
/// <typeparam name="TItem"></typeparam>
public interface IBsStorage<TItem>
{
	/// <summary>아이템 추가</summary>
	/// <param name="item"></param>
	/// <returns>비동기 처리한 태스크</returns>
	Task AddItemAsync(TItem item);
	/// <summary>아이템 삭제</summary>
	/// <param name="item"></param>
	/// <returns>비동기 처리한 태스크</returns>
	Task RemoveItemAsync(TItem item);
	/// <summary>아이템 얻기</summary>
	/// <param name="id">찾을 아이디</param>
	/// <returns>찾은 아이템</returns>
	TItem? GetItem(string id);
}


/// <summary>
/// 컴포넌트 컨테이너 인터페이스
/// </summary>
/// <typeparam name="TItem"></typeparam>
public interface IBsContainer<TItem> : IBsStorage<TItem>
{
	/// <summary>현재 아이템 ID</summary>
	string? CurrentId { get; set; }
	/// <summary>골라둔 아이템</summary>
	TItem? SelectedItem { get; set; }
	/// <summary>아이템 선택</summary>
	/// <param name="item"></param>
	/// <param name="stateChange"></param>
	/// <returns>비동기 처리한 태스크</returns>
	Task SelectItemAsync(TItem? item, bool stateChange = false);
	/// <summary>아이디로 아이템 선택</summary>
	/// <param name="id"></param>
	/// <param name="stateChange"></param>
	/// <returns>비동기 처리한 태스크</returns>
	Task SelectItemAsync(string id, bool stateChange = false);
}


/// <summary>태그 아이템 핸들러</summary>
/// <remarks>컨테이너가 아닌것은 개체를 소유하지 않고 처리만 도와주기 때문</remarks>
public interface IBsTagHandler
{
	/// <summary>
	/// 태그 아이템의 CSS클래스를 설정
	/// </summary>
	/// <param name="item"></param>
	/// <param name="cssc"></param>
	void OnClass(BsTag item, BsCss cssc);
	/// <summary>
	/// 태그 아이템의 렌더 트리를 만듦
	/// </summary>
	/// <param name="item"></param>
	/// <param name="builder"></param>
	void OnRender(BsTag item, RenderTreeBuilder builder);
}


/// <summary>태그 콘텐트 부위</summary>
public enum BsContentRole
{
	Header,
	Footer,
	Content,
}


/// <summary>
/// 태그 콘텐트 에이전시
/// </summary>
public interface IBsContentHandler
{
	/// <summary>
	/// 태그 콘텐트의 CSS클래스를 설정
	/// </summary>
	/// <param name="role"></param>
	/// <param name="content">콘텐트</param>
	/// <param name="cssc">CssCompose</param>
	void OnClass(BsContentRole role, BsContent content, BsCss cssc);

	/// <summary>
	/// 태그 콘텐트의 렌더 트리를 만듦
	/// </summary>
	/// <param name="role">
	/// </param>
	/// <param name="content">콘텐트</param>
	/// <param name="builder">빌드 개체</param>
	void OnRender(BsContentRole role, BsContent content, RenderTreeBuilder builder);
}


/// <summary>
/// 리스트 에이전트
/// </summary>
public interface IBsListAgent
{
	string? Tag { get; }
	string? Class { get; }
}
