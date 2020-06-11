/*
 * 此文件由T4引擎自动生成, 请勿修改此文件中的代码!
 */
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Native.Core.Domain;
using Native.Sdk.Cqp;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using Unity;

namespace Native.App.Export
{
	/// <summary>	
	/// 表示酷Q菜单导出的类	
	/// </summary>	
	public class CQMenuExport	
	{	
		#region --构造函数--	
		/// <summary>	
		/// 由托管环境初始化的 <see cref="CQMenuExport"/> 的新实例	
		/// </summary>	
		static CQMenuExport ()	
		{	
			
			// 调用方法进行实例化	
			ResolveBackcall ();	
		}	
		#endregion	
		
		#region --私有方法--	
		/// <summary>	
		/// 读取容器中的注册项, 进行事件分发	
		/// </summary>	
		private static void ResolveBackcall ()	
		{	
			/*	
			 * Name: 设置	
			 * Function: _OpenWindowA	
			 */	
			if (AppData.UnityContainer.IsRegistered<IMenuCall> ("设置"))	
			{	
				Menu_OpenWindowAHandler += AppData.UnityContainer.Resolve<IMenuCall> ("设置").MenuCall;	
			}	
			
			/*	
			 * Name: 重置教务系统数据库	
			 * Function: _MenuInitEas	
			 */	
			if (AppData.UnityContainer.IsRegistered<IMenuCall> ("重置教务系统数据库"))	
			{	
				Menu_MenuInitEasHandler += AppData.UnityContainer.Resolve<IMenuCall> ("重置教务系统数据库").MenuCall;	
			}	
			
			/*	
			 * Name: 重置日程数据库	
			 * Function: _MenuInitSch	
			 */	
			if (AppData.UnityContainer.IsRegistered<IMenuCall> ("重置日程数据库"))	
			{	
				Menu_MenuInitSchHandler += AppData.UnityContainer.Resolve<IMenuCall> ("重置日程数据库").MenuCall;	
			}	
			
			/*	
			 * Name: 重置Git提醒数据库	
			 * Function: _MenuInitGit	
			 */	
			if (AppData.UnityContainer.IsRegistered<IMenuCall> ("重置Git提醒数据库"))	
			{	
				Menu_MenuInitGitHandler += AppData.UnityContainer.Resolve<IMenuCall> ("重置Git提醒数据库").MenuCall;	
			}	
			
		}	
		#endregion	
		
		#region --导出方法--	
		/*	
		 * Name: 设置	
		 * Function: _OpenWindowA	
		 */	
		public static event EventHandler<CQMenuCallEventArgs> Menu_OpenWindowAHandler;	
		[DllExport (ExportName = "_OpenWindowA", CallingConvention = CallingConvention.StdCall)]	
		public static int Menu_OpenWindowA ()	
		{	
			if (Menu_OpenWindowAHandler != null)	
			{	
				CQMenuCallEventArgs args = new CQMenuCallEventArgs (AppData.CQApi, AppData.CQLog, "设置", "_OpenWindowA");	
				Menu_OpenWindowAHandler (typeof (CQMenuExport), args);	
			}	
			return 0;	
		}	
		
		/*	
		 * Name: 重置教务系统数据库	
		 * Function: _MenuInitEas	
		 */	
		public static event EventHandler<CQMenuCallEventArgs> Menu_MenuInitEasHandler;	
		[DllExport (ExportName = "_MenuInitEas", CallingConvention = CallingConvention.StdCall)]	
		public static int Menu_MenuInitEas ()	
		{	
			if (Menu_MenuInitEasHandler != null)	
			{	
				CQMenuCallEventArgs args = new CQMenuCallEventArgs (AppData.CQApi, AppData.CQLog, "重置教务系统数据库", "_MenuInitEas");	
				Menu_MenuInitEasHandler (typeof (CQMenuExport), args);	
			}	
			return 0;	
		}	
		
		/*	
		 * Name: 重置日程数据库	
		 * Function: _MenuInitSch	
		 */	
		public static event EventHandler<CQMenuCallEventArgs> Menu_MenuInitSchHandler;	
		[DllExport (ExportName = "_MenuInitSch", CallingConvention = CallingConvention.StdCall)]	
		public static int Menu_MenuInitSch ()	
		{	
			if (Menu_MenuInitSchHandler != null)	
			{	
				CQMenuCallEventArgs args = new CQMenuCallEventArgs (AppData.CQApi, AppData.CQLog, "重置日程数据库", "_MenuInitSch");	
				Menu_MenuInitSchHandler (typeof (CQMenuExport), args);	
			}	
			return 0;	
		}	
		
		/*	
		 * Name: 重置Git提醒数据库	
		 * Function: _MenuInitGit	
		 */	
		public static event EventHandler<CQMenuCallEventArgs> Menu_MenuInitGitHandler;	
		[DllExport (ExportName = "_MenuInitGit", CallingConvention = CallingConvention.StdCall)]	
		public static int Menu_MenuInitGit ()	
		{	
			if (Menu_MenuInitGitHandler != null)	
			{	
				CQMenuCallEventArgs args = new CQMenuCallEventArgs (AppData.CQApi, AppData.CQLog, "重置Git提醒数据库", "_MenuInitGit");	
				Menu_MenuInitGitHandler (typeof (CQMenuExport), args);	
			}	
			return 0;	
		}	
		
		#endregion	
	}	
}
