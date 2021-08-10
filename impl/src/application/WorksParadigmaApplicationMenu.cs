using Works.Application.Navigation;
using Works.Localization;
using Works.Web.Common.Icons;
using Works.Application.Infrastructure;


namespace Works.Paradigma
{ 
    public class WorksParadigmaApplicationMenu : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            BuildExampleMenu(context);

        }

        private void BuildExampleMenu(INavigationProviderContext context)
        {
            var menuName = "WorksMainMenu";
            var menu = context.Manager.Menus[menuName] = new MenuDefinition(name: menuName, displayName: new FixedLocalizableString(menuName));

            menu.CreateMenu("Works.Paradigma", IconMaterialDesign.Apps)
                .AddMenu("Tabelas", IconMaterialDesign.Table)
                   .AddMenuItem("Departamentos", "/departamento/manager", IconMaterialDesign.CashMultiple)
                   .AddMenuItem("Pessoa", "/pessoa/manager", IconMaterialDesign.Account)
                .AddMenu("Works.Paradigma", "Challenge", IconMaterialDesign.Table)
                   .AddMenuItem("Desafio BTS/Arvore/TreeNode", "/challenge/treenode", IconMaterialDesign.Tree)
                   .AddMenuItem("Maiores Salario/Departamento", "/challenge/salario", IconMaterialDesign.CashMultiple);

        }




    }


}
