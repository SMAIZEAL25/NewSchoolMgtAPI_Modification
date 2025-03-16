namespace New_School_Management_API.QueryingDB
{
    public class QueriableParameter
    {
      
            private int _pageSize = 15;

            public int StartIndex { get; set; }

            public int PageNumber { get; set; }

            public int PageSize
            {
                get { return _pageSize; }

                set { _pageSize = value; }
            }
        }
    }

