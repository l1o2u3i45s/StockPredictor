﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStructure
{
    public class StockData
    {
        public StockData(int size,string id)
        {
            ID = id;
            Date = new DateTime[size];
            OpenPrice = new double[size];
            ClosePrice = new double[size];
            HightestPrice = new double[size];
            LowestPrice = new double[size];
            Volumn = new double[size];

            KValue = new double[size];
            DValue = new double[size];

            MA120 = new double[size];
            MA60 = new double[size];
            MA20 = new double[size];
            MA5 = new double[size];

            RSI5 = new double[size];
            RSI10 = new double[size];

            RSV = new double[size];
            IsFilter = new bool[size];


        }

        public string ID { get; set; }

        public DateTime[] Date { get; set; }
        public double[] OpenPrice { get; set; }
        public double[] ClosePrice { get; set; }
        public double[] HightestPrice { get; set; }
        public double[] LowestPrice { get; set; }
        public double[] Volumn { get; set; }

        public double[] RSV { get; set; }
        public double[] KValue { get; set; }
        public double[] DValue { get; set; }

        public double[] MA120 { get; set; }
        public double[] MA60 { get; set; }
        public double[] MA20 { get; set; } 
        public double[] MA5 { get; set; }

        public double[] RSI5 { get; set; }
        public double[] RSI10 { get; set; }

        public bool[] IsFilter { get; set; }
    }

    public class StockInfo
    {

        public StockInfo(StockData data,int index,string id,string name)
        {
            ID = id;
            Name = name;

            Date = data.Date[index];
            OpenPrice = data.OpenPrice[index];
            ClosePrice = Math.Round(data.ClosePrice[index],2);
            HightestPrice = Math.Round(data.HightestPrice[index],2);
            LowestPrice = data.LowestPrice[index];
            Volumn = data.Volumn[index];

            RSV = data.RSV[index];
            KValue = data.KValue[index];
            DValue = data.DValue[index];

            MA120 = data.MA120[index];
            MA60 = data.MA60[index];
            MA20 = data.MA20[index];
            MA5 = data.MA5[index];

            RSI5 = data.RSI5[index];
            RSI10 = data.RSI10[index];
        }

        public string ID { get; set; }
        public string Name { get; set; }

        public DateTime Date { get; set; }
        public double OpenPrice { get; set; }
        public double ClosePrice { get; set; }
        public double HightestPrice { get; set; }
        public double LowestPrice { get; set; }
        public double Volumn { get; set; }

        public double RSV { get; set; }
        public double KValue { get; set; }
        public double DValue { get; set; }

        public double MA120 { get; set; }
        public double MA60 { get; set; }
        public double MA20 { get; set; }
        public double MA5 { get; set; }

        public double RSI5 { get; set; }
        public double RSI10 { get; set; }

        public double CurrentClosePrice { get; set; }
        public double CloseDiffValue
        {
            get => Math.Round(CurrentClosePrice - ClosePrice,5); 
        }
        public double GrowRatio
        { 
            get => Math.Round( ((CurrentClosePrice / ClosePrice) -1) * 100, 1);
        }
    }
}
