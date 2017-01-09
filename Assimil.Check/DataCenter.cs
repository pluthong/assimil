using System;
using System.Data;

namespace Assimil
{
   public class DataCenter
    {
        /// <summary>
        /// Fornisce l'accesso alle funzioni di interrogazione al database
        /// </summary>
        protected DataProvider _objProvider = null;

        /// <summary>
        /// Fornisce l'accesso alle funzioni di interrogazione al database
        /// </summary>
        public DataProvider Provider
        {
            get
            {
                if (_objProvider == null)
                    _objProvider = new DataProvider();
                return _objProvider;
            }
        }

        public DataTable GetList(string reg)
        {
            return Provider.StatusCheckForRegion(reg);
        }

        public bool updList(DVLotteryUser user)
        {
            int result = 0;

            try
            {
                 result = Provider.UpdateList(user);
            }
            catch (Exception ex)
            {
                
                throw;
            }

            return result != 0;
        }

        public bool AtNVCList(DVLotteryUser user)
        {
            int result = 0;

            try
            {
                result = Provider.AtNVCList(user);
            }
            catch (Exception ex)
            {
                throw;
            }

            return result != 0;
        }

        public bool TransitList(DVLotteryUser user)
        {
            int result = 0;

            try
            {
                result = Provider.TransitList(user);
            }
            catch (Exception ex)
            {

                throw;
            }

            return result != 0;
        }

    }
}
