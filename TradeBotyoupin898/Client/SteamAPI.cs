using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using SteamAuth;
using TradeBotyoupin898.DataStruct;

namespace TradeBotyoupin898.Client
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
            refreshSession();
            return currentAccount.FetchConfirmations();
        }

        public bool AcceptConfirmation(Confirmation conf)
        {
            return currentAccount.AcceptConfirmation(conf);
        }

        public void AcceptOffer(IOrderData order, bool require2FA)
        {
            var postData = new NameValueCollection
            {
                { "partner", order.GetBuyer().ToString() },
                { "serverid", "1" },
                { "sessionid", currentAccount.Session.SessionID },
                { "tradeofferid", order.GetTradeOfferId() },
                { "captcha", string.Empty }
            };

            refreshSession();
            var cookies = new CookieContainer();
            cookies.Add(new Cookie("sessionid", currentAccount.Session.SessionID, "/", ".steamcommunity.com"));
            cookies.Add(new Cookie("steamLoginSecure", currentAccount.Session.SteamLoginSecure, "/", ".steamcommunity.com")
            {
                HttpOnly = true,
                Secure = true
            });

            SteamWeb.Request($"{APIEndpoints.COMMUNITY_BASE}/tradeoffer/{order.GetTradeOfferId()}/accept", "POST", data: postData, cookies: cookies, referer: $"{APIEndpoints.COMMUNITY_BASE}/tradeoffer/{order.GetTradeOfferId()}");

            if (require2FA)
            {
                var confs = GetConfirmation();
                foreach (var conf in confs)
                {
                    if (conf.Creator != ulong.Parse(order.GetTradeOfferId())) continue;
                    if (!AcceptConfirmation(conf))
                        throw new Exception("SteamAuth 向 Steam 发送 POST 时失败");
                }
            }
        }

        private void refreshSession()
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
