﻿using cc.wnapp.whuHelper.Code;
using cc.wnapp.whuHelper.UI;
using Native.Sdk.Cqp.Interface;
using Unity;

namespace Native.Core
{
	/// <summary>
	/// 酷Q应用主入口类
	/// </summary>
	public class CQMain
	{
		/// <summary>
		/// 在应用被加载时将调用此方法进行事件注册, 请在此方法里向 <see cref="IUnityContainer"/> 容器中注册需要使用的事件
		/// </summary>
		/// <param name="container">用于注册的 IOC 容器 </param>
		public static void Register (IUnityContainer unityContainer)
		{
			unityContainer.RegisterType<IGroupMessage, event_Message>("群消息处理");
			unityContainer.RegisterType<IPrivateMessage, event_Message>("私聊消息处理");
			unityContainer.RegisterType<IMenuCall, OpenWindowA>("设置");
			unityContainer.RegisterType<ICQStartup, event_CQStartup>("酷Q启动事件");
			unityContainer.RegisterType<IAppEnable, cc.wnapp.whuHelper.Code.event_AppStartup>("应用已被启用");
		}
	}
}
