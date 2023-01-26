using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace icAPIAddinEnableDisable
{
    public class ArgsParam
    {
        public class AddinSettingDataSet
        {
            public bool isEnable;
            public string configPath;
            public string guid;
            public string addinName;
            public string addinDescription;
            public AddinSettingDataSet()
            {
                this.isEnable = false;
                this.configPath = string.Empty;
                this.guid = string.Empty;
                this.addinName = string.Empty;
                this.addinDescription = string.Empty;
            }
            public AddinSettingDataSet(bool isEnable, string configPath, string guid, string addinName, string addinDescription)
            {
                this.isEnable = isEnable;
                this.configPath = configPath;
                this.guid = guid;
                this.addinName = addinName;
                this.addinDescription = addinDescription;
            }
        }
    }
}
