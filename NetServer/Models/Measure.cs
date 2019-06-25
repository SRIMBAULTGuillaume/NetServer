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
        DAO dao = new DAO();

        public void generateTable(int size)  
        {
            valueList = new ArrayList();
            valueList = dao.RetreiveValues("average", device, size);
        }

        public void generateTable(int size, int frequence)
        {
            valueList = new ArrayList();
            valueList = dao.RetreiveValues("average", device, size, frequence);
        }
    }

    public class Measure
    {
        public DateTime date;
        public int value;
    }
}