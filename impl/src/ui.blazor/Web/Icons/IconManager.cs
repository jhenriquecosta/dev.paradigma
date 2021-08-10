using Newtonsoft.Json;
using Works.Web.Blazor.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Works.Application.Services.Dto;

namespace Works.Web.Icons
{

    public class IconManager
    {
        static List<ComboBoxItemDto> internalData;
        IXTSysSettings _settings;
        public IconManager(IXTSysSettings settings)
        {
            _settings = settings;
        }
        public List<DataItemCombo> GetIcons(IconSource icon = IconSource.MaterialDesign,IconSize iconSize=IconSize.PX18)
        {
            var lstIcons = new List<DataItemCombo>();
            var filename = $"{icon.GetDescription<IconSource>()}.json";
            filename = Path.Combine(_settings.Folder.Icons, filename);
            var records = JsonConvert.DeserializeObject<List<OxIcon>>(System.IO.File.ReadAllText(filename));
            var _id = 0;
            foreach(var record in records)
            {
                _id++;
                var data = new DataItemCombo();
                var prefix = string.Empty;
               
                if (icon == IconSource.MaterialDesign) prefix = "mdi";
                var suffix = $"{prefix}-{iconSize.GetDescription<IconSize>()}";
                var name = $"{prefix} {prefix}-{record.Name} {suffix}";
                data.Key = _id;
                data.Text = name;
                data.Descricao = record.Name;
                lstIcons.Add(data);
            }
            lstIcons = lstIcons.OrderBy(f=>f.Descricao).ToList();
            return lstIcons;
        }
        public object GetValue(string value)
        {  
          var icon = internalData.FirstOrDefault(f => f.Text.Equals(value));
          if (icon == null) icon = internalData.First();
          return icon.Key;           
        }
        public string GetName(object value)
        {
            var key = value.ToString().ToInt();
            var icon = internalData.FirstOrDefault(f => f.Key.Equals(key));
            if (icon == null) icon = internalData.First();
            return icon.Text;
        }
    }
}
