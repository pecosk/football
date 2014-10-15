using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http.Dispatcher;

namespace Selfhost
{
	public class CustomAssembliesResolver : DefaultAssembliesResolver
	{
		public override ICollection<Assembly> GetAssemblies()
		{
			ICollection<Assembly> baseAssemblies = base.GetAssemblies();

			List<Assembly> assemblies = new List<Assembly>(baseAssemblies);

//			var controllersAssembly = Assembly.LoadFrom(@"C:\libs\controllers\ControllersLibrary.dll");
			var controllersAssembly = Assembly.Load("FootballLeague");

			baseAssemblies.Add(controllersAssembly);

			return assemblies;
		}
	}}

