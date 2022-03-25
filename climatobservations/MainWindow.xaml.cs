using climatobservations.Models;
using climatobservations.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace climatobservations
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {

        DbRepository db;

        public MainWindow()
        {
            InitializeComponent();

            db = new DbRepository();
            PopulateCategory();
        }


        public Observer GetObserver(string observerName)
        {
            string[] subs = observerName.Split(' ');
            string firstName = subs[0];
            string lastName = subs[1];
            var observer = new Observer();
            observer.Firstname = firstName;
            observer.Lastname = lastName;
            return observer;
        }


        private void btnAddObserver_Click(object sender, RoutedEventArgs e)
        {
            var observerName = txtNameObserver.Text;
            var observer = GetObserver(observerName);
            
            db.AddObserver(observer);
        }


        private void btnRemoveObserver_Click(object sender, RoutedEventArgs e)
        {
            var observerName = txtNameObserver.Text;
            var observer = GetObserver(observerName);

            db.RemoveObserver(observer);
            txtNameObserver.Clear();
        }


        private void btnShowObserver_Click(object sender, RoutedEventArgs e)
        {
            var observers = db.GetObserver();

            foreach (var observer in observers.OrderBy(x => x.Lastname)) // Sorterar efternamn https://stackoverflow.com/questions/188141/listt-orderby-alphabetical-order Linq-expression.
            {
                lstbox.Items.Add(observer.Firstname + " " + observer.Lastname);
            }
        }

        private void btnAddObservation_Click(object sender, RoutedEventArgs e) 
        {
            if (lstbox.SelectedItem == null || lstboxCategory.SelectedItem == null)
            {
                MessageBox.Show($"Lägg till observatör, kategori och mätpunkt.");
            }
            else
            {
                decimal value; 
                string observerName = Convert.ToString(lstbox.SelectedItem);
                string categoryName = Convert.ToString(lstboxCategory.SelectedItem);
                try
                {
                    value = Convert.ToDecimal(txtMeasurement.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show($"Ange mätvärde i siffror.");
                    return; 
                }

                var observer = GetObserver(observerName);
                var observers = db.GetObserver();

                observer = observers.Where(x => x.Lastname == observer.Lastname && x.Firstname == observer.Firstname).FirstOrDefault(); // Hämta observer(id) https://stackoverflow.com/questions/15536830/linq-firstordefault-then-select Linq-expression.
                var observation = db.GetObservationByObserverId(observer);

                if(observation == default(Observation)) // Om värdet är null så läggs ett datum för observation in
                {
                    db.AddObservation(observer, DateTime.Today); //Lägger autodatum i observation
                    observation = db.GetObservationByObserverId(observer);
                }

                if (categoryName == "Fjällripa vinterdräkt")
                {
                    try
                    {
                        decimal snowdepth = Convert.ToDecimal(txtMeasurementSnow.Text);
                        var snowcategory = GetCategory("Snödjup");
                        db.AddMeasurement(observation, snowcategory, snowdepth);
                        ShowObservation(observer, observation, snowcategory, snowdepth);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show($"Ange snödjup i cm.");
                        return;
                    }    
                }
                var category = GetCategory(categoryName);
                db.AddMeasurement(observation, category, value);
                ShowObservation(observer, observation, category, value);
                MessageBox.Show($"Tack! Din observation är nu tillagd.");
                lstboxObservation.Visibility = Visibility.Visible;
            }
        }


        private Category GetCategory(string categoryName)
        {  
            var categories = db.GetCategory();
            var category = categories.Where(x => x.Name == categoryName).FirstOrDefault();
            return category; 
        }


        private void lstbox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            txtMeasurement.Visibility = Visibility.Visible; 
            lbl.Visibility = Visibility.Visible;
            lstboxCategory.Visibility = Visibility.Visible;
            btnAddObservation.Visibility = Visibility.Visible;
        }

        private void lstbox_MouseDoubleClick2(object sender, MouseButtonEventArgs e)
        {
            var observation = Convert.ToString(lstboxObservation.SelectedItem);
            var string_array = observation.Split(':');
            lstbox.SelectedItem = string_array[0];
            lstboxCategory.SelectedItem = string_array[2];
            txtMeasurement.Text = string_array[3]; 
        }

        private void PopulateCategory()
        {
            var categories = db.GetCategory();

            foreach (var category in categories)
            {
                lstboxCategory.Items.Add(category.Name);
            }
        }

   
        private void lstboxCategory_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            string categoryName = Convert.ToString(lstboxCategory.SelectedItem);

            if (categoryName == "Fjällripa vinterdräkt")
            {
                txtMeasurementSnow.Visibility = Visibility.Visible;
            }
            else
            {
                txtMeasurementSnow.Visibility = Visibility.Hidden;
            }

        }

        private void ShowObservation(Observer observer, Observation observation, Category category, decimal value)
        {
            lstboxObservation.Items.Add(observer.Firstname + " " + observer.Lastname + ":" + observation.Date.ToString("yyyy-MM-dd") + ":" + category.Name + ":" + value);
        }
    }
}

     