using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using System.Linq;
using System;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.ComponentModel;
using Microsoft.AspNetCore.Components.Web;
using System.Security.Cryptography;

namespace RuleEditor.Client.Shared.Components.ViewModels
{
	public class DataGridModel<TItem> : ComponentBase
	{
		private List<DataGridRow<TItem>> m_rows = new List<DataGridRow<TItem>>();
		private List<DataGridRow<TItem>> m_currRows = new List<DataGridRow<TItem>>();
		protected string LoadingOverlayClassNameValue { get { return this.IsLoading ? "loading-overlay-visible" : "loading-overlay-hidden"; } }
		private Dictionary<string, PropertyInfo> m_propMap = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);
		private Dictionary<object, bool> m_checkedItems = new Dictionary<object, bool>();


		[Parameter]
		public List<TItem> Items { get; set; }

		[Parameter]
		public string TableClass { get; set; } = "";

		[Parameter]
		public string TableStyle { get; set; } = "";

		[Parameter]
		public string RowClass { get; set; } = "blazor-row-item";

		[Parameter]
		public string RowStyle { get; set; } = "";

		/// <summary>
		/// Required, even if we don't use it so we can bind to Columns
		/// </summary>
		[Parameter]
		public RenderFragment ChildContent { get; set; }

		[Parameter]
		public SelectionType SelectionType { get; set; }

		[Parameter]
		public EventCallback<IEnumerable<TItem>> OnCheckedItemsChanged { get; set; }

		[Parameter]
		public int PageSize { get; set; } = 10;

		[Parameter]
		public int MaxPagesToDisplay { get; set; } = 5;

		[Parameter]
		public bool IsLoading { get; set; }

		[Parameter]
		public bool ShowDebuggingInformation { get; set; } = false;

		protected List<DataGridColumnModel<TItem>> Columns { get; set; } = new List<DataGridColumnModel<TItem>>();

		public int CurrentPageIndex { get; set; }
		public int CurrentPage
		{
			get { return CurrentPageIndex + 1; }
		}

		public int TotalPages
		{
			get
			{
				return (int)Math.Ceiling(
					(double)((Items == null || Items.Count() == 0) ? 1 : Items.Count()) /
					(double)(PageSize == 0 ? 1 : PageSize));
			}
		}

		public int StartPage;
    	public int EndPage;

		public void SetPagerSize(int direction)
    	{
			if (direction == 1 && EndPage < TotalPages)
			{
				StartPage = EndPage + 1;
				if (EndPage + MaxPagesToDisplay < TotalPages)
				{
					EndPage = StartPage + MaxPagesToDisplay - 1;
				}
				else
				{
					EndPage = TotalPages;
				}
			}
			else if (direction == -1 && StartPage > 1)
			{
				EndPage = StartPage - 1;
				StartPage = StartPage - MaxPagesToDisplay;
			}
		}

		public void AddColumn(DataGridColumnModel<TItem> col)
		{
			_debug += $"Column added - {col.PropertyName} ";
			this.Columns.Add(col);
			UpdatePropertyMappings();
			UpdateCurrentItems();
		}

		public void RemoveColumn(DataGridColumnModel<TItem> col)
		{
			_debug += "Column removed ";
			this.Columns.Remove(col);
		}

		protected List<DataGridRow<TItem>> Rows { get { return m_currRows; } }

		protected string _debug { get; set; } = "_debuggin' ";

		protected override void OnInitialized()
		{
			StartPage = 1;

			SetPagerSize(1);

			_debug += $"initialized items: {Items.Count}, cols:{Columns.Count}, isLoading:{IsLoading} ";
			base.OnInitialized();			

		}

		protected override void OnAfterRender(bool firstRender)
		{
			_debug += $"onAfterRender first:{firstRender} ";

		}

		protected string GetItemCell(DataGridColumnModel<TItem> column, DataGridRow<TItem> row)
		{
			var value = GetItemValue(column.PropertyName, row.Item);
			var valueString = value.ToString();
			return valueString;
		}

		protected override void OnParametersSet()
		{
			base.OnParametersSet();
			_debug += $"onParametersSet columns:{Columns.Count} ";
			m_rows = Items.Select(i => new DataGridRow<TItem>() { Item = i }).ToList();
			UpdateCurrentItems();
		}

		protected bool ShowCheckboxes { get { return this.SelectionType == SelectionType.MultipleCheckboxes || this.SelectionType == SelectionType.SingleCheckbox; } }
		protected bool ShowSelections { get { return this.SelectionType == SelectionType.MultipleRows || this.SelectionType == SelectionType.SingleRow; } }

		protected object GetItemValue(string propertyName, TItem item)
		{
			PropertyInfo prop = null;
			if (!m_propMap.TryGetValue(propertyName, out prop))
			{
				_debug += $"no map found {propertyName} ";// throw new Exception($"Problem sorting by {propertyName} - no mapping found.");
				return "missing";
			}
			else
			{
				var value = prop.GetValue(item);
				return value;
			}
		}

		protected void UpdateCurrentItems()
		{
			if (this.Items == null || !this.Items.Any())
			{
				return;
			}

			IEnumerable<DataGridRow<TItem>> query = null;
			var sortedCols = this.GetSortedColumns();
			if (!sortedCols.Any())
			{
				//if no sorts, default to sort by first column
				query = this.m_rows;//.OrderBy(r => GetItemValue(this.Columns.First().PropertyName, r.Item));
			}
			else
			{
				query = this.m_rows.Where(r => 1 == 1);
				foreach (var sort in sortedCols)
				{
					if (sort.CurrentSortType == SortType.Ascending)
					{
						query = query.OrderBy(r => GetItemValue(sort.PropertyName, r.Item));
					}
					else if (sort.CurrentSortType == SortType.Descending)
					{
						query = query.OrderByDescending(r => GetItemValue(sort.PropertyName, r.Item));
					}
				}

			}
			m_currRows = query
				.Skip(CurrentPageIndex * PageSize)
				.Take(PageSize)
				.ToList();

			StateHasChanged();
		}

		protected bool IsColumnSortable(DataGridColumnModel<TItem> col)
		{
			var sortableCols = this.GetSortedColumns();
			var result = sortableCols.Any(s => s.PropertyName.Equals(col.PropertyName, StringComparison.OrdinalIgnoreCase));
			_debug += $"is {col.PropertyName} sortable? {result} ";
			return result;
		}

		protected async Task OnRowCheckedAsync(ChangeEventArgs e, DataGridRow<TItem> row)
		{
			bool isChecked = bool.Parse(e.Value.ToString());
			row.IsSelected = isChecked;
			await HandleRowSelectionChangedAsync(row);
		}


		protected async Task OnRowSelectedAsync(DataGridRow<TItem> row)
		{
			if (this.SelectionType == SelectionType.SingleCheckbox || this.SelectionType == SelectionType.SingleRow)
			{
				this.Rows.ForEach(r => r.IsSelected = false);
			}
			row.IsSelected = !row.IsSelected;
			await HandleRowSelectionChangedAsync(row);
		}

		protected async Task HandleRowSelectionChangedAsync(DataGridRow<TItem> newRowItemState)
		{
			StateHasChanged();
			await this.OnCheckedItemsChanged.InvokeAsync(this.m_rows.Where(r => r.IsSelected).Select(r => r.Item));
		}

		protected void OnFirstClicked(EventArgs e)
		{
			this.CurrentPageIndex = 0;
			UpdateCurrentItems();			
		}

		protected void OnNextClicked(EventArgs e)
		{
			if (CurrentPage < TotalPages)
			{
				this.CurrentPageIndex++;
				UpdateCurrentItems();
				SetPagerSize(1);
			}
		}

		protected void OnPrevClicked(EventArgs e)
		{
			if (CurrentPage > 1)
			{
				this.CurrentPageIndex--;
				UpdateCurrentItems();
				SetPagerSize(-1);
			}
		}

		protected void OnLastClicked(EventArgs e)
		{
			this.CurrentPageIndex = TotalPages - 1;
			UpdateCurrentItems();
			SetPagerSize(-1);
		}

		protected void OnPageSet(EventArgs e, int page)
		{
			_debug += $" pageSet param:{page} ";
			this.CurrentPageIndex = page - 1;
			UpdateCurrentItems();
		}

		protected void OnHeaderClicked(DataGridColumnModel<TItem> col)
		{
			var match = this.GetSortedColumns().FirstOrDefault(s => s.PropertyName.Equals(col.PropertyName, StringComparison.OrdinalIgnoreCase));
			if (match != null)
			{
				SortType currSortType = match.CurrentSortType,
					newSortType = SortType.None;

				if (currSortType == SortType.None || currSortType == SortType.Ascending)
				{
					newSortType = SortType.Descending;
				}
				else
				{
					newSortType = SortType.Ascending;
				}

				_debug += $"Sorting - current:{currSortType}, new:{newSortType} ";

				//reset the other columns' sorts
				ResetSorts();

				match.SetSortType(newSortType);
				_debug += $"sortcheck: {match.CurrentSortType} ";
			}
			UpdateCurrentItems();
		}

		private void UpdatePropertyMappings()
		{
			lock (m_propMap)
			{
				var typeProps = typeof(TItem).GetProperties();
				foreach (var col in Columns)
				{
					if (col.PropertyName != null)
					{
						var match = typeProps.FirstOrDefault(p => p.Name.Equals(col.PropertyName, StringComparison.OrdinalIgnoreCase));
						if (match == null)
						{
							throw new Exception($"Unknown mapping to property {col.PropertyName}");
						}
						else
						{
							if (m_propMap.ContainsKey(col.PropertyName))
							{
								m_propMap[col.PropertyName] = match;
							}
							else
							{
								m_propMap.Add(col.PropertyName, match);
							}
						}
					}
				}
			}
		}


		private IEnumerable<DataGridColumnModel<TItem>> GetSortedColumns()
		{
			return this.Columns.Where(c => c.IsSortable);
		}

		private void ResetSorts()
		{
			foreach (var col in this.Columns)
			{
				if (col.IsSortable)
				{
					col.SetSortType(SortType.None);
				}
			}
		}
	}



	public class DataGridRow<TItem>
	{
		public TItem Item { get; set; }

		public bool IsSelected { get; set; }
	}



	public enum SelectionType
	{
		None = 0,
		SingleRow = 1,
		MultipleRows = 2,
		SingleCheckbox = 3,
		MultipleCheckboxes = 4
	}

	/// <summary>
	/// Indicates that a field is a key to uniquely identify a row
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class DataGridRowKeyAttribute : System.Attribute
	{

	}
}