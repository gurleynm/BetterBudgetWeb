namespace BetterBudgetWeb.Data
{
    public class Security
    {
        private string cryptoURL = "https://api.nasdaq.com/api/quote/{0}/info?assetclass=crypto";
        private string stockURL = "https://api.nasdaq.com/api/quote/{0}/info?assetclass=stocks";
        private string etfURL = "https://api.nasdaq.com/api/quote/{0}/info?assetclass=etf";

        private string optionURL = "https://api.nasdaq.com/api/quote/{0}/option-chain?assetclass=stocks&limit=100&fromdate={1}&todate={1}&excode=oprac&callput={2}&money=all&type=all";
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public double NumShares { get; set; }
        public double AvgCost { get; set; }
        public double Cost { get; set; }
        public double Value => NumShares * Cost;
        public double StrikePrice { get; set; }
        public string SecType { get; set; }
        public string Person { get; set; }
        public DateTime DateOfSecurity { get; set; } = DateTime.Now;
        public Security() { }
        public Security(string name, string type)
        {
            Name = name;
            SecType = type;
            Task.Run(() => SetCost(name, type));
        }
        public Security(string name, string type, double numShares)
        {
            Name = name;
            SecType = type;
            NumShares = numShares;
            Task.Run(async () => await SetCost(name, type));
        }

        public async Task SetCost(string name, string type)
        {
            double cost = 0;

            string QueryUri;

            switch (type)
            {
                case "Crypto":
                    QueryUri = string.Format(cryptoURL, name);
                    break;

                case "ETF":
                    QueryUri = string.Format(etfURL, name);
                    break;

                case "Call":
                case "Put":
                    QueryUri = string.Format(optionURL, name);
                    break;

                default:
                    QueryUri = string.Format(stockURL, name);
                    break;
            }
            HttpClient client = new HttpClient();


            /*
            client.DefaultRequestHeaders.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,* /*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            client.DefaultRequestHeaders.Add("cookie", "ak_bmsc=8224C99B54109EB3D13A1E0C51F1EAD1~000000000000000000000000000000~YAAQll3aF2CFkZOAAQAARIhlpg+0ofhXtzZJz2fJW7SwYZqJy3KeI29ZULCFoFkisMi382rRPxH+QZ2omteT6bcCQXhR3W36+IG35KKql3Kvr9629Ab/6N4d+WUwTw4lygU6A6S7Q1AHz07S45QP+cIURSB0ZgeU9EAasfH4ga1SI5Pl6pkpvLtgaG2vgVsXnAIjETat+aKSGnGAWwdOao6G+KsDvLIMNL6vNmdOz9VuRhqvGniQi4xnYhGdc7ei46veE8/q5JnU/ngfKvDxNs1mtJILRHMMry3FnQhzQzHBeqnPoqInqbmkaGf810la6cvb5hT8FOyg/Yx6rXBlmke0IK7MFD1UIobiYfLCIcVmv0UR1zJ4R94c/FBNXSENe+qDBievqD6Me2Q=; bm_sv=5837ED9A4BCF0C823BAAEDCFFFB03C53~YAAQll3aF3CHkZOAAQAAprdlpg+nHnIVpVNKF2gL4rH+VfgfuJ9FOYNl5raJYQKnHtxrfaxwik6EADA9UZkN1Z2II2jnNcaiDYOFNM6qKgHha3rEPZzizoLfdXGijTsC+6vAXJcGlhse3JZQr2xoXDJobBmLNNLAyv/vksfn3yvyliImaJuZmn9/sSVlt0SndAMu1d/R14P1/49UMwNVKEax+np1IAO8yLdN4J71mBbulcLYIC7d6KcTY+Srekw/~1");
            client.DefaultRequestHeaders.Add("accept-encoding", "gzip,deflate,br");
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, QueryUri);
            */
            //client.DefaultRequestHeaders.Add("access-control-allow-origin", "*");
            try
            {/*
                var client = new RestClient("http://api.nasdaq.com/api/quote/GME/info?assetclass=stocks");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Cookie", "ak_bmsc=6ED399F24C9EA0BB1B464FD722BDEF26~000000000000000000000000000000~YAAQ3F3aF4kCXpSAAQAABKViqg93fjDNQJ5yHEi+FECF2d222pj45b64xJgH+e4Y0N+jS0VZG0BP5l81Db/wC2pmKbJ3gq+aEdR1Nn8Slj9jHvoSclkCR6n2iEqWXryaqQmPenwaXNr9z7LuoquANdW4g5ubZEndqcf7R3HhQgE/5PZiHaBZc/dQaO77kVaoWbkskDVS48qL+rHcKnA1pDfZEJt6XOS0ac3NTe0TpXt6p7VDA3vZMWZgPrQ4UcRbJ6PuFJXk6WvuL+x+rXOCFanWaISCH2nH7D/bp3mffmSaRdMEtBrF1/fHlqoxZ3v7A73KM+mjYBniXBDHYKVjuUkLh1+KKNCgzZt5zwsSPUEitz6ID4BBlLBSZqweWnUslLfv");
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                */
                var response = await client.GetStringAsync("https://api.nasdaq.com/api/quote/gme/option-chain?assetclass=stocks&limit=100&fromdate=2022-06-03&todate=2022-12-31&excode=oprac&callput=call&money=all&type=all");
                Name = response;
                Cost = SecurityHelper.Help(type, response);
            } catch(Exception ex)
            {

            }

            var c = Cost;
        }
    }
}
