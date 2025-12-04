using System;
using System.Windows.Input;

namespace TravelRecordApp.ViewModel
{
	public class HomeViewModel
	{

        public NewTravelCommand NewTravelCommand { get; set; }

		public HomeViewModel()
		{
            NewTravelCommand = new NewTravelCommand(this);
		}

        public void NewTravelNavigation()
        {
            App.Current.MainPage.Navigation.PushAsync(new NewTravelPage());
        }
	}

    public class NewTravelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private HomeViewModel vM;

        public NewTravelCommand(HomeViewModel vm)
        {
            vM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {

            vM.NewTravelNavigation();
        }
    }
}

