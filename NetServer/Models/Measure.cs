using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetServer.Models
{
    public class ResultAdapter
    {
        public string device;
        public ArrayList valueList;

        public void generateTable(int size)
        {
            valueList = new ArrayList();
            for (int i = 0; i < size; i++)
            {
                Measure measure = new Measure();
                measure.date = new DateTime(2019+((((i/60)/24)/30)/12),(6+(((i/60)/24)/30))%12+1, (21+((i/60)/24))%30+1, (7+(i/60))%24, (0+i)%60, 0);
                measure.value = 20;
                valueList.Add(measure);
            }
        }
    }

    public class Measure
    {
        public DateTime date;
        public int value;
    }
}