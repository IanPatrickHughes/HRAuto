﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;


namespace HRAutomation.DAL
{
    public static class DatabaseHelper
    {
        public const string ConnectionStringName = "HRAutomationConnectionString";

        public static AutomationDataDataContext GetRequestData()
        {
            var db = new AutomationDataDataContext(ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString);
            return db;
        }


        #region /// <HR InterfaceRecords helpers> ...
            public static void Update<T>(T obj, Action<T> update) where T : class
            {
                using (var db = GetRequestData())
                {
                    db.GetTable<T>().Attach(obj);
                    update(obj);
                    db.SubmitChanges();
                }
            }
            public static void UpdateAll<T>(List<T> items, Action<T> update) where T : class
            {
                using (var db = GetRequestData())
                {
                    Table<T> table = db.GetTable<T>();
                    foreach (T item in items)
                    {
                        table.Attach(item);
                        update(item);
                    }

                    db.SubmitChanges();
                }
            }
            public static void Delete<T>(T entity) where T : class, new()
            {
                using (var db = GetRequestData())
                {
                    Table<T> table = db.GetTable<T>();
                    table.Attach(entity);
                    table.DeleteOnSubmit(entity);
                    db.SubmitChanges();
                }
            }

            public static void Insert<T>(T obj) where T : class
            {
                using (var db = GetRequestData())
                {
                    db.GetTable<T>().InsertOnSubmit(obj);
                    db.SubmitChanges();
                }
            }
        #endregion

    }
}
