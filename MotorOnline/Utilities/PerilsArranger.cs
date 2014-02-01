using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MotorOnline.Library.Entity;

namespace MotorOnline
{
    public class PerilsArranger
    {
        public static List<Perils> SortWithCTPL(List<Perils> perils)
        {
            List<int> ids = new List<int>();
            ids.AddRange(new int[] { 187, 274, 182, 191, 194, 195, 184 });

            List<Perils> newList = new List<Perils>();
            foreach (int id in ids)
            {
                newList.Add(perils.Where(x => x.PerilID == id).FirstOrDefault());
            }
            return newList;
        }

        public static List<Perils> SortWithoutCTPL(List<Perils> perils)
        {
            List<int> ids = new List<int>();
            ids.AddRange(new int[] { 274, 182, 191, 194, 195, 184 });

            List<Perils> newList = new List<Perils>();
            foreach (int id in ids)
            {
                newList.Add(perils.Where(x => x.PerilID == id).FirstOrDefault());
            }
            return newList;
        }


        public static List<TransactionPeril> SortWithCTPL(List<TransactionPeril> perils)
        {
            List<int> ids = new List<int>();
            ids.AddRange(new int[] { 187, 274, 182, 191, 194, 195, 184 });

            List<TransactionPeril> newList = new List<TransactionPeril>();
            foreach (int id in ids)
            {
                newList.Add(perils.Where(x => x.PerilID == id).FirstOrDefault());
            }
            return newList;
        }

        public static List<TransactionPeril> SortWithoutCTPL(List<TransactionPeril> perils)
        {
            List<int> ids = new List<int>();
            ids.AddRange(new int[] { 274, 182, 191, 194, 195, 184 });

            List<TransactionPeril> newList = new List<TransactionPeril>();
            foreach (int id in ids)
            {
                newList.Add(perils.Where(x => x.PerilID == id).FirstOrDefault());
            }
            return newList;
        }
    }
}