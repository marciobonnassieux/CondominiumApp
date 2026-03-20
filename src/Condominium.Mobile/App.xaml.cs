using Condominium.Mobile.Pages;

namespace Condominium.Mobile;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

#pragma warning disable CS0618
        var nav = new NavigationPage(new Pages.LoginPage());
        nav.BarBackgroundColor = Color.Parse("#2c3e50"); // Cinza chumbo no Topo
        nav.BarTextColor = Color.Parse("#F1C40F"); // Textos em Amarelo Ouro
        MainPage = nav;
#pragma warning restore CS0618
    }
}