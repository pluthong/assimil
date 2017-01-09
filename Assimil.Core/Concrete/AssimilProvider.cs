
using Newtonsoft.Json;
using Assimil.Domain;
using System.Linq;
using System.Collections.Generic;

namespace Assimil.Core
{
    public class AssimilProvider : IAssimil
    {

        public PagingAssimil Get(int page, int pageSize)
        {
            var json = Helpers.GetJson("Assimil.Core.Resource.lesson.json");

            List<MinimalAssimil> list = JsonConvert.DeserializeObject<List<MinimalAssimil>>(json);

            int totalItem = list.Count();

            var querypage = list.Skip((page - 1) * pageSize).Take(pageSize).ToArray();

            list = querypage.ToList();

            PagingAssimil p = new PagingAssimil { Assimils = list, Assimil_count = totalItem };

            return p;
        }

      
    }
}
