﻿using System;
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
using System.Windows.Shapes;
using DataTransferObjects;
using LogicLayer;
using LogicLayerInterfaces;
namespace WPFPresentationLayer.AdoptionPages
{
    /// <summary>
    /// Creator: Mohamed Elamin
    /// Created: 2020/02/29
    /// Approver: Awaab Elamin, 2020/03/03
    ///
    ///  This class has interaction logic for the Interviewer
    ///  page.
    public partial class pgAdoptionInterviewer : Page
    {
      
        private IAdoptionInterviewerManager _adoptionInterviewerManager;

        /// <summary>
        /// Creator: Mohamed Elamin
        /// Created: 2020/02/29
        /// Approver: Awaab Elamin, 2020/03/03
        /// 
        /// This This constructor method for In-Home Interviewer page.
        /// 
        /// </summary>
        ///
        /// <remarks>
        /// Updater Name
        /// Updated: yyyy/mm/dd 
        /// Update: ()
        /// </remarks>
        /// <param name=""></param>
        /// <returns></returns>
        public pgAdoptionInterviewer()
        {
            InitializeComponent();
            _adoptionInterviewerManager = new AdoptionInterviewerManager();
           
        }
        /// <summary>
        /// Creator: Mohamed Elamin
        /// Created: 2020/02/29
        /// Approver: Awaab Elamin, 2020/03/03
        /// 
        /// This method fills in data for Interviewer Data Gird.
        /// 
        /// </summary>
        ///
        /// <remarks>
        /// Updater Name
        /// Updated: yyyy/mm/dd 
        /// Update: ()
        /// </remarks>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        private void populateAdoptionAappointmentList()
        {
            dgAdoptionAappointmentList.ItemsSource =
                _adoptionInterviewerManager.SelectAdoptionAappointmentsByAppointmentType();
            dgAdoptionAappointmentList.Columns[0].Header = "Appointment ID";
            dgAdoptionAappointmentList.Columns[1].Header = "Adoption Application ID";
            dgAdoptionAappointmentList.Columns[2].Header = "Appointment Type ID";
            dgAdoptionAappointmentList.Columns[3].Header = "DateTime";
            dgAdoptionAappointmentList.Columns[4].Header = "Notes";
            dgAdoptionAappointmentList.Columns[5].Header = "Decision";
            dgAdoptionAappointmentList.Columns[6].Header = "Location ID";
            dgAdoptionAappointmentList.Columns[7].Header = "Active";

        }

        /// <summary>
        /// Creator: Mohamed Elamin
        /// Created: 2020/02/29
        /// Approver: Awaab Elamin, 2020/03/03
        /// 
        /// This is an Event on the Page first loads.
        /// </summary>
        ///
        /// <remarks>
        /// Updater Name
        /// Updated: yyyy/mm/dd 
        /// Update: ()
        /// </remarks>
        /// <param name=" sender"></param>
        /// <param name=" e"></param>
        /// <returns></returns>  
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            populateAdoptionAappointmentList();
        }

        /// <summary>
        /// Creator: Mohamed Elamin
        /// Created: 2020/02/29
        /// Approver: Awaab Elamin, 2020/03/03
        /// 
        /// This is an Event on the Open Button clicked.
        /// </summary>
        ///
        /// <remarks>
        /// Updater Name
        /// Updated: yyyy/mm/dd 
        /// Update: ()
        /// </remarks>
        /// <param name=" sender"></param>
        /// <param name=" e"></param>
        /// <returns></returns>  
        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            AdoptionAppointment selectedAappointment =
               (AdoptionAppointment)dgAdoptionAappointmentList.SelectedItem;
            if (selectedAappointment == null)
            {
                MessageBox.Show("Please select an appliction to see the details");
                return;

            }


            this.NavigationService?.Navigate(new pgAdoptionInterviewerNotes
                (selectedAappointment, _adoptionInterviewerManager));


        }

    }
}