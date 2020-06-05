using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuleEditor.Client.Shared.Components.ViewModels
{
	public class DataGridColumnModel<TItem> : ComponentBase, IDisposable
	{
		private string m_headerStyle, m_cellStyle;
		private SortType m_currSortType;

		[Parameter]
		public string PropertyName { get; set; }
		[Parameter]
		public string Label { get; set; }
		[Parameter]
		public double WidthPercent { get; set; }
		[Parameter]
		public string HeaderClass { get; set; } = "";
		[Parameter]
		public string HeaderStyle { get { return GetHeaderStyle(); } set { m_headerStyle = value; } }
		[Parameter]
		public string CellClass { get; set; } = "";
		[Parameter]
		public string CellStyle { get { return GetCellStyle(); } set { m_cellStyle = value; } }
		[Parameter]
		public RenderFragment<TItem> Template { get; set; }
		[Parameter]
		public bool IsSortable { get; set; }
		[Parameter]
		public SortType InitialSortType { get; set; }

		[CascadingParameter]
		public DataGridModel<TItem> DataTable { get; set; }

		public SortType CurrentSortType
		{
			get { return m_currSortType; }
		}


		protected override void OnInitialized()
		{
			m_currSortType = this.InitialSortType;
			DataTable.AddColumn(this);
			base.OnInitialized();
		}

		public void SetSortType(SortType type)
		{
			this.m_currSortType = type;
			StateHasChanged();
		}

		private string GetHeaderStyle()
		{
			return this.WidthPercent == 0 ?
				$"{m_headerStyle}" :
				$"width:{this.WidthPercent}%;{m_headerStyle}";
		}

		private string GetCellStyle()
		{
			return this.WidthPercent == 0 ?
				$"{m_cellStyle}" :
				$"width:{this.WidthPercent}%;{m_cellStyle}";
		}
		public void Dispose()
		{
			DataTable.RemoveColumn(this);
		}
	}

	public enum SortType
	{
		None = 0,
		Ascending = 1,
		Descending = 2
	}
}
