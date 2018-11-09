using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Documents {
    public class AggregatedData {

        private int numOpened;

        /// <summary>
        /// Number of times the user has opened the link
        /// </summary>
        public int NumOpened {
            get { return numOpened; }
            set { numOpened = value; }
        }

        private int numSearched;

        /// <summary>
        /// Number of times searched for
        /// </summary>
        public int NumSearched {
            get { return numSearched; }
            set { numSearched = value; }
        }

        private int numReviews;

        /// <summary>
        /// Number of reviews. This is added to automatically
        /// </summary>
        public int NumReviews {
            get { return numReviews; }
            set { numReviews = value; }
        }


        private double reviewScore;

        /// <summary>
        /// Average of scores given. Also adds to NumReviews
        /// </summary>
        public double ReviewScore {
            get { return reviewScore; }
            set {
                numReviews++;
                reviewScore += (value - reviewScore)/numReviews;
            }
        }
    }
}