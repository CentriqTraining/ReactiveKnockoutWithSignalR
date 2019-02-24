using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Concurrent;
using ReactiveKnockoutVersion2.Models;

namespace ReactiveKnockoutVersion2
{
    //  Final Hub
    [HubName("reactive")]
    public class ReactiveHub : Hub
    {
        private MVC_ReactiveContext db = new MVC_ReactiveContext();

        #region "Step 6 code changes"
        /// <summary>
        /// Create a Static Threadsafe Dictionary to keep track
        /// of each connection and the employees they have locked
        /// </summary>
        private static ConcurrentDictionary<string, List<int>>
            _mappings = new ConcurrentDictionary<string, List<int>>();

        /// <summary>
        /// When a client connects - adds them to the Dictionary
        /// that keeps track of each connection and the employees
        /// that they have locked.  The key is their connectionId 
        /// and the value is a List of employee IDs that they have locked
        /// </summary>
        /// <returns></returns>
        public override System.Threading.Tasks.Task OnConnected()
        {
            _mappings.TryAdd(Context.ConnectionId, new List<int>());
            return base.OnConnected();
        }

        /// <summary>
        /// When a client disconnects - remove them from the Dictionary
        /// that keeps track of each connection and the employees
        /// that they have locked.  
        /// </summary>
        /// <returns></returns>
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            //  Gather a list of all the employees this client has locked
            //  into an array
            int[] ids = _mappings[Context.ConnectionId].ToArray();

            //  run through them, calling unlock.  This will both
            //  update the DB and also alert all other clients that 
            //  the employee is now unlocked.
            foreach (var item in ids)
            {
                Unlock(item);
            }

            //  Now remove the entire item from the dictionary
            List<int> x;
            _mappings.TryRemove(Context.ConnectionId, out x);

            return base.OnDisconnected(stopCalled);
        }
      
        #endregion

        #region "Step 3 code changes"
        /// <summary>
        /// This method calls the js function updateEmployee in each client 
        /// telling them to update this property
        /// </summary>
        /// <param name="empId">Employee Id to update</param>
        /// <param name="propertyName">The propertName that changed</param>
        /// <param name="propertyValue">The new value as object</param>
        public void SendUpdate(int empId, string propertyName, object propertyValue)
        {
            this.Clients.Others.updateEmployee(empId, propertyName, propertyValue);
        }
        #endregion

        #region "Step 5/6 code changes"
        /// <summary>
        /// This method finds the employee by the Id passed int
        /// and Changes the islocked value and calls the js function in each client
        /// called lockEmployee
        /// </summary>
        /// <param name="id">Employee Id to lock</param>
        public void Lock(int id)
        {
            var emp = FindEmployeeById(id);
            if (emp != null)
            {
                SetLockStatus(emp, true);
                var name = Context.User.Identity.Name;
                Clients.Others.lockEmployee(emp.Id, name);
            }

            //  Add this ID to the list of locked employees for this
            //  connection  *  Added in step 6
            _mappings[Context.ConnectionId].Add(emp.Id);
        }

        /// <summary>
        /// This method finds the employee by the Id passed int
        /// and Changes the islocked value and calls the js function in each client
        /// called lockEmployee.
        /// </summary>
        /// <param name="id">Employee ID to unlock</param>
        public void Unlock(int id)
        {
            var emp = FindEmployeeById(id);
            if (emp != null)
            {
                SetLockStatus(emp, false);
                Clients.Others.unlockEmployee(emp.Id);
            }

            //  remove this ID from the list of locked employees
            //  for this connection  * Added in step 6
            _mappings[Context.ConnectionId].Remove(emp.Id);
        }

        private Employee FindEmployeeById(int id)
        {
            // Find the employee in the DB context
            return db.Employees
                .Where(e => e.Id == id)
                .FirstOrDefault();
        }

        private void SetLockStatus(Employee emp, bool SetToLocked)
        {
            // changed the value of IsLocked to what was passed in 
            // and make a call to SaveChanges to persist to Db
            emp.IsLocked = SetToLocked;
            emp.LockHolder = emp.IsLocked ? Context.User.Identity.Name : null;
            db.SaveChanges();
        }
        #endregion
    }
}

