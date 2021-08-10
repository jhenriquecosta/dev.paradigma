using Syncfusion.Blazor.DropDowns;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Works.Application.Services;
using Works.Application.Services.Dto;
using System.Linq.Expressions;

namespace Works.Web.Blazor.Ui.Sf.Inputs
{
   
    public class BaseWorkComboBox<TEntity> : WorksComboBoxBase<TEntity,object>
    {
       
        protected static SfDropDownList<int?, ComboBoxItemDto> InternalComboBox;
        [Inject] protected IWorksLookUpService LookupService { get; set; }
        protected internal int? InternalValue { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await Initialize();
        }
        public SfDropDownList<int?, ComboBoxItemDto> InternalComponent => InternalComboBox;
        public void SetEnabled(bool enabled)
        {
            if (InternalComboBox != null)
            {
                InternalComboBox.Enabled = enabled;
                Enabled = enabled;
                OnChanged();
            }
        }
         

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            this.Value ??= 0;
            InternalValue = Value.ChangeType<int?>();
            if (InternalValue == 0) InternalValue = null;            

        }

        protected async Task Initialize()
        {
            try
            {   await BuilderDataSourceAsync();

                if (InternalComboBox != null)
                {

                    if (!InternalComboBox.IsRendered)
                    {
                        var objId = InternalComboBox.GetObjectId();
                        if (objId.IsNull())
                        {

                            if (!ObjectID.IsNullOrWhiteSpace())
                            {
                                InternalComboBox.ID = ObjectID;
                            }
                            else
                            {
                                ObjectID = InternalComboBox.ID;
                            }
                            InternalComboBox.SetObjectId(ObjectID);
                        }
                    }
                   
                }

            }
            catch (Exception ex)
            {
                ErrorMessages.Add(ex.Message);
            }
        }
        protected async Task BuilderDataSourceAsync()
        {
            if (DataSource == null)
            {
                if (typeof(TEntity).IsEnum)
                {  
                     DataSource =  WorksEnum.ToComboBox<TEntity>(typeof(TEntity));
                    
                }
                else
                {
                     DataSource = await LookupService.GetAllAsync<TEntity>();
                }
            }
        }

        protected void OnFireBlur(Microsoft.AspNetCore.Components.Web.FocusEventArgs eventArgs)
        {
           
        }

        protected void OnValueChanged(ChangeEventArgs<int?> args)
        {
            try
            {
                if (args.Value != null)
                {
                    if (args.Value > 0)
                    {
                        var rsFound = DataSource.FirstOrDefault(f => f.FieldValue == args.Value);
                        if (rsFound != null)
                        {
                            var record = (TEntity)rsFound.FieldData;
                            ValueChanged.InvokeAsync(record);
                        }
                        else
                        {
                            ValueChanged.InvokeAsync(default(TEntity));
                        }
                    }
                    else
                    {
                        ValueChanged.InvokeAsync(default(TEntity));
                    }
                }
                else
                {

                    ValueChanged.InvokeAsync(default(TEntity));

                }
            }
            catch (Exception ex)
            {
              //  Toast.ShowError(ex.Message, "Erro");
              ErrorMessages.Add(ex.Message);
            }
          
        }
    }
}