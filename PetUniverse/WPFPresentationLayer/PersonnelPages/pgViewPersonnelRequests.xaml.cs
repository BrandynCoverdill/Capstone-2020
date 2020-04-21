﻿using DataTransferObjects;
using LogicLayer;
using LogicLayerInterfaces;
using PresentationUtilityCode;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WPFPresentationLayer.PersonnelPages
{
    /// <summary>
    /// Interaction logic for ViewPersonnelRequests.xaml
    /// </summary>
    public partial class ViewPersonnelRequests : Page
    {
        IRequestManager _requestManager;
        IShiftManager _shiftManager;
        IUserManager _userManager;
        PetUniverseUser _user;

        /// <summary>
        /// NAME : Kaleb Bachert
        /// DATE: 2/20/2020
        /// APPROVER: Lane Sandburg
        /// 
        /// This is the default constructor
        /// </summary>
        /// <remarks>
        /// UPDATED BY: NA
        /// UPDATED NA
        /// CHANGE: NA
        /// 
        /// </remarks>

        public ViewPersonnelRequests()
        {
            _requestManager = new RequestManager();
            _shiftManager = new ShiftManager();
            _userManager = new UserManager();
            _user = new PetUniverseUser();

            InitializeComponent();
        }

        /// <summary>
        /// NAME : Kaleb Bachert
        /// DATE: 2/20/2020
        /// APPROVER: Lane Sandburg
        /// 
        /// This is the detailed constructor
        /// </summary>
        /// <remarks>
        /// UPDATED BY: NA
        /// UPDATED NA
        /// CHANGE: NA
        /// 
        /// </remarks>

        public ViewPersonnelRequests(PetUniverseUser user)
        {
            _requestManager = new RequestManager();
            _shiftManager = new ShiftManager();
            _userManager = new UserManager();
            _user = user;

            InitializeComponent();
        }

        /// <summary>
        /// CREATOR: Kaleb Bachert
        /// CREATED: 2020/2/14
        /// APPROVER: MOST LIKELY SCRAPPING THIS
        /// 
        /// This method is called when the search bar in personnel Requests, gets focus
        /// </summary>
        /// <remarks>
        /// UPDATER: NA
        /// UPDATED: NA
        /// UPDATE: NA
        /// 
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void txtSearchRequests_GotFocus(object sender, RoutedEventArgs e)
        //{
        //    ((TextBox)sender).Text = string.Empty;
        //    ((TextBox)sender).Foreground = new SolidColorBrush(Colors.Black);
        //    ((TextBox)sender).FontStyle = FontStyles.Normal;

        //    //Removes this event handler after one call. Won't keep deleting text.
        //    ((TextBox)sender).GotFocus -= txtSearchRequests_GotFocus;
        //}

        /// <summary>
        /// CREATOR: Kaleb Bachert
        /// CREATED: 2020/2/14
        /// APPROVER: Lane Sandburg
        /// 
        /// This method is called when the Open Requests DataGrid is done generating Columns
        /// </summary>
        /// <remarks>
        /// UPDATER: NA
        /// UPDATED: NA
        /// UPDATE: NA
        /// 
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgOpenRequests_AutoGeneratedColumns(object sender, EventArgs e)
        {
            dgOpenRequests.Columns.RemoveAt(9);
            dgOpenRequests.Columns.RemoveAt(8); //Approved By
            dgOpenRequests.Columns.RemoveAt(6);
            dgOpenRequests.Columns.RemoveAt(5); //Date Approved
            dgOpenRequests.Columns.RemoveAt(4);
            dgOpenRequests.Columns.RemoveAt(3);
            dgOpenRequests.Columns.RemoveAt(0);
            dgOpenRequests.Columns[0].Header = "Request Type";
            dgOpenRequests.Columns[1].Header = "Request Creation Date";
            dgOpenRequests.Columns[2].Header = "Requesting Employee Email";
        }

        /// <summary>
        /// CREATOR: Kaleb Bachert
        /// CREATED: 2020/2/14
        /// APPROVER: Lane Sandburg
        /// 
        /// This method is called when the Requests Tab in Personnel, is loaded
        /// </summary>
        /// <remarks>
        /// UPDATER: NA
        /// UPDATED: NA
        /// UPDATE: NA
        /// 
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgOpenRequests_Loaded(object sender, RoutedEventArgs e)
        {
            populateRequestList(!(bool)chkViewClosedRequests.IsChecked);
        }

        /// <summary>
        /// CREATOR: Kaleb Bachert
        /// CREATED: 2020/2/14
        /// APPROVER: Lane Sandburg
        /// 
        /// This method is called to update the dgOpenRequests ItensSource
        /// </summary>
        /// <remarks>
        /// UPDATER: NA
        /// UPDATED: NA
        /// UPDATE: NA
        /// 
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void populateRequestList(bool open)
        {
            try
            {
                dgOpenRequests.ItemsSource = _requestManager.RetrieveRequestsByStatus(open);
            }
            catch (Exception ex)
            {
                WPFErrorHandler.ErrorMessage(ex.InnerException.Message, ex.Message);
            }
        }


        /// <summary>
        /// CREATOR: Kaleb Bachert
        /// CREATED: 2020/2/16
        /// APPROVER: Lane Sandburg
        /// 
        /// This method is called when btnRequestDetails is clicked, determines request type, 
        /// and changes canvas accordingly
        /// </summary>
        /// <remarks>
        /// UPDATER: NA
        /// UPDATED: NA
        /// UPDATE: NA
        /// 
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRequestDetails_Click(object sender, RoutedEventArgs e)
        {
            Window window = null;
            RequestVM selectedRequest = (RequestVM)dgOpenRequests.SelectedItem;

            if (selectedRequest != null)
            {
                switch (selectedRequest.RequestTypeID)
                {
                    case "Time Off":
                        canPersonnelRequests.Visibility = Visibility.Hidden;
                        canTimeOffRequestDetails.Visibility = Visibility.Visible;
                        TimeOffRequestVM selectedTimeOffRequest = null;

                        try
                        {
                            selectedTimeOffRequest = _requestManager.RetrieveTimeOffRequestByRequestID(
                                selectedRequest.RequestID);
                        }
                        catch (Exception)
                        {
                            throw;
                        }

                        selectDateRange(selectedTimeOffRequest);
                        setTimeOffRequestTextFields(selectedTimeOffRequest);
                        break;

                    case "Schedule Change":
                        canPersonnelRequests.Visibility = Visibility.Hidden;
                        canScheduleChangeRequestDetails.Visibility = Visibility.Visible;
                        ScheduleChangeRequestVM selectedScheduleChangeRequest = null;

                        try
                        {
                            selectedScheduleChangeRequest = _requestManager.RetrieveScheduleChangeRequestByRequestID(
                                selectedRequest.RequestID);
                            selectedScheduleChangeRequest.EmployeeWorkingEmail = selectedRequest.RequestingEmail;
                        }
                        catch (Exception)
                        {
                            throw;
                        }

                        setScheduleChangeRequestTextFields(selectedScheduleChangeRequest);
                        break;

                    default:
                        WPFErrorHandler.ErrorMessage("REQUEST TYPE NOT IMPLEMENTED!", "Implementation");
                        break;
                }
            }
            else
            {
                MessageBox.Show("Make a Selection First!");
            }

            if (window != null)
            {
                if (window.ShowDialog() == true)
                {
                    populateRequestList(!(bool)chkViewClosedRequests.IsChecked);
                }
            }
        }

        /// <summary>
        ///  CREATOR: Kaleb Bachert
        ///  CREATED: 2020/2/17
        ///  APPROVER: Lane Sandburg
        ///  
        ///  This button simply closes the window.
        /// </summary>
        /// <remarks>
        /// UPDATER: NA
        /// UPDATED: NA
        /// UPDATE: NA
        /// 
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTimeOffCancel_Click(object sender, RoutedEventArgs e)
        {
            canTimeOffRequestDetails.Visibility = Visibility.Hidden;
            canPersonnelRequests.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///  CREATOR: Kaleb Bachert
        ///  CREATED: 2020/4/8
        ///  APPROVER: Lane Sandburg
        ///  
        ///  This button simply closes the window.
        /// </summary>
        /// <remarks>
        /// UPDATER: NA
        /// UPDATED: NA
        /// UPDATE: NA
        /// 
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnScheduleChangeCancel_Click(object sender, RoutedEventArgs e)
        {
            canScheduleChangeRequestDetails.Visibility = Visibility.Hidden;
            canPersonnelRequests.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///  CREATOR: Kaleb Bachert
        ///  CREATED: 2020/2/19
        ///  APPROVER: Lane Sandburg
        ///  
        ///  This method is called to select the dates mentioned in the request
        ///  on the calendar control
        /// </summary>
        /// <remarks>
        /// UPDATER: NA
        /// UPDATED: NA
        /// UPDATE: NA
        /// 
        /// </remarks>
        /// <param name="request"></param>

        private void selectDateRange(TimeOffRequestVM request)
        {
            calTimeOffDateRange.SelectedDates.Clear();

            DateTime startDate = Convert.ToDateTime(request.EffectiveStart);
            if (request.EffectiveEnd != null && request.EffectiveEnd != "")
            {
                DateTime endDate = Convert.ToDateTime(request.EffectiveEnd);

                calTimeOffDateRange.SelectedDates.AddRange(startDate, endDate);
            }
            else
            {
                calTimeOffDateRange.SelectedDate = startDate;
            }

            calTimeOffDateRange.DisplayDateStart = Convert.ToDateTime(request.EffectiveStart);
            calTimeOffDateRange.DisplayDateEnd = Convert.ToDateTime(request.EffectiveEnd);
        }

        /// <summary>
        ///  CREATOR: Kaleb Bachert
        ///  CREATED: 2020/2/19
        ///  APPROVER: Lane Sandburg
        ///  
        ///  This method is called to fill in the text fields for
        ///  Time Off Request details
        /// </summary>
        /// <remarks>
        /// UPDATER: NA
        /// UPDATED: NA
        /// UPDATE: NA
        /// 
        /// </remarks>
        /// <param name="request"></param>
        private void setTimeOffRequestTextFields(TimeOffRequestVM request)
        {
            //Gets the Date only from EffectiveStart
            txtTimeOffStartDate.Text = Convert.ToDateTime(request.EffectiveStart).ToString("d");

            //Gets the Date only from EffectiveEnd
            if (request.EffectiveEnd != null && request.EffectiveEnd != "")
            {
                txtTimeOffEndDate.Text = Convert.ToDateTime(request.EffectiveEnd).ToString("d");
            }

            //Gets the Time only from EffectiveStart
            txtTimeOffStartTime.Text = Convert.ToDateTime(request.EffectiveStart).TimeOfDay.ToString();

            //Gets the Time only from EffectiveEnd
            txtTimeOffEndTime.Text = Convert.ToDateTime(request.EffectiveEnd).TimeOfDay.ToString();
        }

        /// <summary>
        ///  CREATOR: Kaleb Bachert
        ///  CREATED: 2020/4/9
        ///  APPROVER: Lane Sandburg
        ///  
        ///  This method is called to fill in the text fields for
        ///  Schedule Change Request details
        /// </summary>
        /// <remarks>
        /// UPDATER: NA
        /// UPDATED: NA
        /// UPDATE: NA
        /// 
        /// </remarks>
        /// <param name="request"></param>
        private void setScheduleChangeRequestTextFields(ScheduleChangeRequestVM request)
        {
            //Sets the Employee Working field
            txtScheduleChangeEmployeeScheduled.Text = request.EmployeeWorkingEmail;

            //Sets the Shift Department field
            txtScheduleChangeDepartment.Text = request.DepartmentID;

            //Sets the Shift Date field
            txtScheduleChangeShiftDate.Text = request.Date;

            //Sets the Shift Start Time field
            txtScheduleChangeStartTime.Text = request.StartTime;

            //Sets the Shift End Time field
            txtScheduleChangeEndTime.Text = request.EndTime;

            //Sets the list of Users who can take the shift, clearing it first, and setting the default value
            cmbScheduleChangeReplacementEmployees.Items.Clear();
            cmbScheduleChangeReplacementEmployees.Items.Add("-- Select a Replacement --");

            List<PetUniverseUser> replacementUsers;

            replacementUsers = _userManager.RetrieveUsersAbleToWork(
                        Convert.ToDateTime(request.Date),
                        (Convert.ToDateTime(request.Date)).ToString("dddd"),
                        Convert.ToDateTime(request.StartTime),
                        Convert.ToDateTime(request.EndTime),
                        request.Role);

            foreach (PetUniverseUser user in replacementUsers)
            {
                cmbScheduleChangeReplacementEmployees.Items.Add(user.Email);
            }

            cmbScheduleChangeReplacementEmployees.SelectedItem = "-- Select a Replacement --";
        }

        /// <summary>
        ///  CREATOR: Kaleb Bachert
        ///  CREATED: 2020/2/19
        ///  APPROVER: Lane Sandburg
        ///  
        ///  This button will pop up a confirmation of approval, and approve the request
        /// </summary>
        /// <remarks>
        /// UPDATER: NA
        /// UPDATED: NA
        /// UPDATE: NA
        /// 
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTimeOffApprove_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult confirmApproval = MessageBox.Show("Are you sure you want to approve this request?",
                                      "Confirm Approval?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirmApproval == MessageBoxResult.Yes)
            {
                try
                {
                    RequestVM selectedRequest = ((RequestVM)dgOpenRequests.SelectedItem);
                    int requestsAffected = _requestManager.ApproveRequest(selectedRequest.RequestID, _user.PUUserID, "Time Off");

                    if (1 == requestsAffected)
                    {
                        WPFErrorHandler.SuccessMessage("Successfully Approved Request!");

                        TimeOffRequestVM selectedTimeOffRequest = _requestManager.RetrieveTimeOffRequestByRequestID(selectedRequest.RequestID);

                        //Adding approved request to a table for TimeOff for the schedule to read from
                        ActiveTimeOff activeTimeOff = new ActiveTimeOff()
                        {
                            UserID = selectedRequest.RequestingEmployeeID,
                            StartDate = Convert.ToDateTime(selectedTimeOffRequest.EffectiveStart),
                            EndDate = Convert.ToDateTime(selectedTimeOffRequest.EffectiveEnd)
                        };

                        _requestManager.AddActiveTimeOff(activeTimeOff);
                    }
                    else if (1 < requestsAffected)
                    {
                        WPFErrorHandler.ErrorMessage("MULTIPLE REQUESTS AFFECTED! ALERT SYSTEM ADMINISTRATOR!", "Database");
                    }
                    else if (0 == requestsAffected)
                    {
                        WPFErrorHandler.ErrorMessage("The request was either already approved, or could not be found!", "Request");
                    }
                }
                catch (Exception ex)
                {
                    WPFErrorHandler.ErrorMessage(ex.InnerException.Message, ex.Message);
                }

                //Return to requests page with data refreshed
                canTimeOffRequestDetails.Visibility = Visibility.Hidden;
                canPersonnelRequests.Visibility = Visibility.Visible;
                populateRequestList(!(bool)chkViewClosedRequests.IsChecked);
            }
        }

        /// <summary>
        ///  CREATOR: Kaleb Bachert
        ///  CREATED: 2020/4/15
        ///  APPROVER: Lane Sandburg
        ///  
        ///  This button will pop up a confirmation of approval, and approve the request
        ///  It will also check to see if this change will put the User's hours over 40 for that week 
        /// 
        /// </summary>
        /// <remarks>
        /// UPDATER: NA
        /// UPDATED: NA
        /// UPDATE: NA
        /// 
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnScheduleChangeApprove_Click(object sender, RoutedEventArgs e)
        {
            //No Replacement Employee is selected
            if (cmbScheduleChangeReplacementEmployees.SelectedItem.Equals("-- Select a Replacement --"))
            {
                WPFErrorHandler.ErrorMessage("You must first select a replacement Employee. If no replacement Employees appear, none fit the criteria for replacement.", "Input");
            }
            //Replacement Employee is selected
            else
            {
                //The date, startTime and endTime of the shift, and the length of the shift in hours
                DateTime shiftDate = Convert.ToDateTime(txtScheduleChangeShiftDate.Text);
                TimeSpan startTime = TimeSpan.Parse(txtScheduleChangeStartTime.Text);
                TimeSpan endTime = TimeSpan.Parse(txtScheduleChangeEndTime.Text);
                double shiftHours;

                //If the shift stays in a single day
                if (endTime > startTime)
                {
                    shiftHours = (endTime - startTime).TotalHours;
                }
                //The Shift starts the previous night, and ends in the morning
                else
                {
                    //Last second of the night
                    TimeSpan dayEnd = new TimeSpan(23, 59, 59);
                    //First second of the morning
                    TimeSpan dayStart = new TimeSpan(0, 0, 0);

                    //Get difference between end of shift and start of day, add it to difference between start of shift and end of night
                    shiftHours = (endTime - dayStart).TotalHours + (dayEnd - startTime).TotalHours;
                }

                try
                {
                    //Converts selected Email (replacement User) to a PetUniverseUser
                    PetUniverseUser replacementUser = _userManager.getUserByEmail(cmbScheduleChangeReplacementEmployees.SelectedItem.ToString());

                    //Gets the start and end date of the schedule the shift is in, used to determine the hours worked that week
                    ScheduleWithHoursWorked scheduleContainingTheShift = _shiftManager.RetrieveScheduleHoursByUserAndDate(replacementUser.PUUserID, shiftDate);

                    //This will be the hours worked is the request is officially approved
                    double projectedHoursWorked;

                    //The week of the schedule that this shift is in, 1 or 2
                    int weekNumber;

                    //If shiftDate is before the second Sunday of the Schedule
                    if (shiftDate < scheduleContainingTheShift.ScheduleStartDate.AddDays(7))
                    {
                        projectedHoursWorked = scheduleContainingTheShift.FirstWeekHours + shiftHours;
                        weekNumber = 1;
                    }
                    //ShiftDate is on or after the second Sunday of the Schedule
                    else
                    {
                        projectedHoursWorked = scheduleContainingTheShift.SecondWeekHours + shiftHours;
                        weekNumber = 2;
                    }

                    MessageBoxResult confirmApproval;

                    //The User will have more than 40 hours that week if this is approved
                    if (projectedHoursWorked > 40)
                    {
                        confirmApproval = MessageBox.Show("This change will set " + replacementUser.FirstName.ToUpper() + " " + replacementUser.LastName.ToUpper() +
                            "'s" + Environment.NewLine + "hours for the week to: " + projectedHoursWorked + Environment.NewLine +
                            "Are you certain you want to commit this schedule change?" + Environment.NewLine + "THIS ACTION CANNOT BE UNDONE!",
                          "Confirm Approval?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    }
                    //Hours will not go over 40
                    else
                    {
                        confirmApproval = MessageBox.Show("Are you certain you want to give " + replacementUser.FirstName.ToUpper() + " " + replacementUser.LastName.ToUpper() +
                            " this shift?" + Environment.NewLine + "THIS ACTION CANNOT BE UNDONE!", "Confirm Approval?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    }

                    //Request has been approved and ready to be replaced with the selected replacement Employee
                    if (confirmApproval == MessageBoxResult.Yes)
                    {
                        //Approve the request
                        RequestVM selectedRequest = ((RequestVM)dgOpenRequests.SelectedItem);
                        int requestsAffected = _requestManager.ApproveRequest(selectedRequest.RequestID, _user.PUUserID, "Schedule Change");

                        if (1 == requestsAffected)
                        {
                            PetUniverseUser originalEmployeeWorking = _userManager.getUserByEmail(txtScheduleChangeEmployeeScheduled.Text);

                            //Getting the ScheduleChangeRequest from the Request
                            ScheduleChangeRequestVM selectedScheduleChangeRequest = _requestManager.RetrieveScheduleChangeRequestByRequestID(selectedRequest.RequestID);


                            //Decrease old employee's hours worked, increase the new employee's hours, save the new employee in the Shift's User field
                            _shiftManager.EditEmployeeHoursWorked(originalEmployeeWorking.PUUserID, scheduleContainingTheShift.ScheduleID, weekNumber, shiftHours * -1);
                            _shiftManager.EditEmployeeHoursWorked(replacementUser.PUUserID, scheduleContainingTheShift.ScheduleID, weekNumber, shiftHours);
                            _shiftManager.EditShiftUserWorking(selectedScheduleChangeRequest.ShiftID, replacementUser.PUUserID, originalEmployeeWorking.PUUserID);

                            WPFErrorHandler.SuccessMessage("Successfully Approved Request!");
                        }
                        else if (1 < requestsAffected)
                        {
                            WPFErrorHandler.ErrorMessage("MULTIPLE REQUESTS AFFECTED! ALERT SYSTEM ADMINISTRATOR!", "Database");
                        }
                        else if (0 == requestsAffected)
                        {
                            WPFErrorHandler.ErrorMessage("The request was either already approved, or could not be found!", "Request");
                        }

                    }
                }
                catch (Exception ex)
                {
                    WPFErrorHandler.ErrorMessage(ex.InnerException.Message, ex.Message);
                }

                //Return to requests page with data refreshed
                canScheduleChangeRequestDetails.Visibility = Visibility.Hidden;
                canPersonnelRequests.Visibility = Visibility.Visible;
                populateRequestList(!(bool)chkViewClosedRequests.IsChecked);
            }
        }

        /// <summary>
        ///  CREATOR: Kaleb Bachert
        ///  CREATED: 2020/2/19
        ///  APPROVER: Lane Sandburg
        ///  
        ///  Click event for chkViewClosedRequests to update which Requests re in the DataGrid
        /// </summary>
        /// <remarks>
        /// UPDATER: NA
        /// UPDATED: NA
        /// UPDATE: NA
        /// 
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkViewClosedRequests_Click(object sender, RoutedEventArgs e)
        {
            populateRequestList(!(bool)chkViewClosedRequests.IsChecked);
        }
    }
}