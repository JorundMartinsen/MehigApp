using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Documents {
    public class AggregatedData {

        private int numOpened;

        public int NumOpened {
            get { return numOpened; }
            set { numOpened = value; }
        }

        private int numSearched;

        public int NumSearched {
            get { return numSearched; }
            set { numSearched = value; }
        }

        private int numReviews;

        public int NumReviews {
            get { return numReviews; }
            set { numReviews = value; }
        }


        private double reviewScore;

        public double ReviewScore {
            get { return reviewScore; }
            set {
                numReviews++;
                reviewScore += (value - reviewScore)/numReviews;
            }
        }



    }
}