using SteamAuth;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;

namespace TradeBotyoupin898
{
    internal class SteamAPI
    {
        private SteamGuardAccount[] allAccounts;
        private SteamGuardAccount currentAccount;
        private Manifest manifest;

        public SteamAPI()
        {
            manifest = Manifest.GetManifest();
            allAccounts = manifest.GetAllAccounts();
            currentAccount = allAccounts.Single();
        }

        public Confirmation[] GetConfirmation()
        {
            RefreshSession();
            return currentAccount.FetchConfirmations();
        }

        public bool AcceptConfirmation(Confirmation conf)
        {
            return currentAccount.AcceptConfirmation(conf);
        }

        public void AcceptOffer(OrderData order)
        {
            var postData = new NameValueCollection();
            postData.Add("partner", order.OtherSteamId.ToString());
            postData.Add("serverid", "1");
            postData.Add("sessionid", currentAccount.Session.SessionID);
            postData.Add("tradeofferid", order.SteamOfferId);
            postData.Add("captcha", string.Empty);

            RefreshSession();
            CookieContainer cookies = new CookieContainer();
            cookies.Add(new Cookie("sessionid", currentAccount.Session.SessionID, "/", ".steamcommunity.com"));
            cookies.Add(new Cookie("steamLoginSecure", currentAccount.Session.SteamLoginSecure, "/", ".steamcommunity.com")
            {
                HttpOnly = true,
                Secure = true
            });

            SteamWeb.Request($"{APIEndpoints.COMMUNITY_BASE}/tradeoffer/{order.SteamOfferId}/accept", "POST", data: postData, cookies: cookies, referer: $"{APIEndpoints.COMMUNITY_BASE}/tradeoffer/{order.SteamOfferId}");
        }

        private void RefreshSession()
        {
            if (currentAccount.RefreshSession())
            {
                manifest.SaveAccount(currentAccount);
            }
            else
            {
                Console.WriteLine("Steam登录过期，请手动刷新");
            }
        }
    }
}
