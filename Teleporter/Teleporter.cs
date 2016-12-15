using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ElectronicObserver.Window;
using ElectronicObserver.Window.Plugins;
using Fiddler;

namespace Teleporter
{
	public class Teleporter : ServerPlugin
	{
		public override string MenuTitle => "Teleporter";

		public override bool RunService(FormMain main)
		{
			FiddlerApplication.BeforeRequest += FiddlerApplication_BeforeRequest;
			return true;
		}

		public override string Version
		{
			get { return "<BUILD_VERSION>"; }
		}

		private Regex _cookieRegionRegex = new Regex(@"ckcy=\d+", RegexOptions.Compiled);
		private Regex _cookieLanguageRegex = new Regex(@"cklg=[^;]+", RegexOptions.Compiled);

		private void FiddlerApplication_BeforeRequest(Session oSession)
		{
			string cookie = oSession.oRequest["Cookie"];

			var cookieRegion = _cookieRegionRegex.Match(cookie);
			if (cookieRegion.Success)
			{
				cookie = cookie.Replace(cookieRegion.Value, "ckcy=1");
			}

			var cookieLanguage = _cookieLanguageRegex.Match(cookie);
			if (cookieLanguage.Success)
			{
				cookie = cookie.Replace(cookieLanguage.Value, "cklg=welcome");
			}
			oSession.oRequest["Cookie"] = cookie;
		}
	}
}
