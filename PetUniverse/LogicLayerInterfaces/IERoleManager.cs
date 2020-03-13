﻿using DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayerInterfaces
{
    /// <summary>
    /// <summary>
    /// Creator: Chase Schutle
    /// Created: 2/7/2020
    /// Approver: Kaleb Bachert
    /// 
    /// Interface for ERoleManager
    /// </summary>
    /// <remarks>
    /// Updater: Chase Schulte
    /// Updated: 2/7/2020
    /// Update: Removed RetrieveByERoleID
    /// 
    /// </remarks>
    /// </summary>
    public interface IERoleManager
    {
        /// <summary>
        /// <summary>
        /// Creator: Chase Schutle
        /// Created: 2/7/2020
        /// Approver: Kaleb Bachert
        /// 
        /// interface method for adding a ERole
        /// </summary>
        /// <remarks>
        /// Updater: 
        /// Updated: 
        /// Update: 
        /// 
        /// </remarks>
        /// <param name="eRole"></param>
        /// <returns></returns>
        bool AddERole(ERole eRole);
        /// <summary>
        /// <summary>
        /// Creator: Chase Schutle
        /// Created: 2/7/2020
        /// Approver: Kaleb Bachert
        /// 
        /// interface method for retrieving all ERole
        /// </summary>
        /// <remarks>
        /// Updater: 
        /// Updated: 
        /// Update: 
        /// 
        /// </remarks>
        /// <returns></returns>
        List<ERole> RetrieveAllERoles();
        /// <summary>
        /// <summary>
        /// Creator: Chase Schutle
        /// Created: 2/7/2020
        /// Approver: Kaleb Bachert
        /// 
        /// interface method for editing a ERole
        /// </summary>
        /// <remarks>
        /// Updater: 
        /// Updated: 
        /// Update: 
        /// 
        /// </remarks>
        /// <param name="oldERole"></param>
        /// <param name="newERole"></param>
        /// <returns></returns>
        bool EditERole(ERole oldERole, ERole newERole);
        /// <summary>
        /// <summary>
        /// Creator: Chase Schutle
        /// Created: 2/7/2020
        /// Approver: Kaleb Bachert
        /// 
        /// interface method for deleting a ERole
        /// </summary>
        /// <remarks>
        /// Updater: 
        /// Updated: 
        /// Update: 
        /// 
        /// </remarks>
        /// <param name="eRoleID"></param>
        /// <returns></returns>
        bool DeleteERole(string eRoleID);
        /// <summary>
        /// <summary>
        /// Creator: Chase Schutle
        /// Created: 2/7/2020
        /// Approver: Kaleb Bachert
        /// 
        /// interface method for deactivating a ERole
        /// </summary>
        /// <remarks>
        /// Updater: 
        /// Updated: 
        /// Update: 
        /// 
        /// </remarks>
        /// <param name="eRoleID"></param>
        /// <returns></returns>
        bool DeactivateERole(string eRoleID);
        /// <summary>
        /// <summary>
        /// Creator: Chase Schutle
        /// Created: 2/7/2020
        /// Approver: Kaleb Bachert
        /// 
        /// interface method for activating a ERole
        /// </summary>
        /// <remarks>
        /// Updater: 
        /// Updated: 
        /// Update: 
        /// 
        /// </remarks>
        /// <param name="eRoleID"></param>
        /// <returns></returns>
        bool ActivateERole(string eRoleID);
        /// <summary>
        /// <summary>
        /// Creator: Chase Schutle
        /// Created: 2/7/2020
        /// Approver: Kaleb Bachert
        /// 
        /// interface method for finding all Active/Inactive ERoles
        /// </summary>
        /// <remarks>
        /// Updater: 
        /// Updated: 
        /// Update: 
        /// 
        /// </remarks>
        /// <param name="active"></param>
        /// <returns></returns>
        List<ERole> RetrieveERolesByActive(bool active = true);
    }
}
