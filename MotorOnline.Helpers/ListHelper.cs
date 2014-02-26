﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotorOnline.Helpers
{
    public static class ListHelper
    {
        public static List<List<T>> SplitList<T>(IEnumerable<T> values, int groupSize, int? maxCount)
        {
            List<List<T>> result = new List<List<T>>();
            // Quick and special scenario
            if (values.Count() <= groupSize)
            {
                result.Add(values.ToList());
            }
            else
            {
                List<T> valueList = values.ToList();
                int startIndex = 0;
                int count = valueList.Count;
                int elementCount = 0;

                while (startIndex < count && (!maxCount.HasValue || (maxCount.HasValue && startIndex < maxCount)))
                {
                    elementCount = (startIndex + groupSize > count) ? count - startIndex : groupSize;
                    result.Add(valueList.GetRange(startIndex, elementCount));
                    startIndex += elementCount;
                }
            }


            return result;
        }
    }
}
