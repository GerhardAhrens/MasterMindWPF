namespace MasterMindWPF.Beispiele
{
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;

    /*
    DataService service = new();
    Factory.RegisterTransient<ViewId>(ViewId.Dashboard, () => new DashboardControl(service));
    Factory.RegisterSingleton<WindowId>(WindowId.Login, () => new LoginWindow());
    Factory.RegisterSingleton<NormalClassId>(NormalClassId.SingletonClass, () => new SingletonClass());

    UserControl dashboard = Factory.Get<UserControl, ViewId>(ViewId.Dashboard);
    LoginWindow logWindow = Factory.Get<LoginWindow, WindowId>(WindowId.Login);
    SingletonClass normalClass = Factory.Get<SingletonClass, NormalClassId>(NormalClassId.SingletonClass);
     */

    public class DataService
    {
        public Guid Id { get; } = Guid.NewGuid();
    }

    public class DashboardControl : UserControl
    {
        public DataService Service { get; }

        public DashboardControl(DataService service)
        {
            this.Service = service;

            Debug.WriteLine($"Die Instanz {this.GetType().Name} erstellt | Service: {service.Id}");
        }
    }

    public class LoginWindow : Window
    {
        public LoginWindow()
        {
            Debug.WriteLine($"Die Instanz {this.GetType().Name} erstellt");
        }
    }

    public class SingletonClass : SingletonBase<SingletonClass>
    {
        public SingletonClass()
        {
            Debug.WriteLine($"Die Instanz {this.GetType().Name} erstellt");
        }
    }

    public enum ViewId
    {
        Dashboard,
    }

    public enum WindowId
    {
        Login,
    }

    public enum NormalClassId
    {
        SingletonClass,
    }
}
