﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace GdutWeixin.Models.Library
{
	[XmlRoot("tbody")]
    public class LibraryTableResult
    {
        public static List<Book> Parse(string tbody)
        {
            List<Book> books = new List<Book>();
            Book book = null;
            using (var reader = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(tbody))))
            {
                string currentElementName = null;
                int tdCount = 0;
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            currentElementName = reader.Name;
                            if (reader.Name == "tr")
                            {
                                book = new Book();
                                tdCount = 0;
                            }
                            else if (reader.Name == "td")
                            {
                                tdCount++;
                            }
                            else if (reader.Name == "a")
                            {
                                book.Url = reader.GetAttribute("href");
                            }
                            break;
                        case XmlNodeType.Text:
                            if (currentElementName == "a")
                            {
                                book.Title = reader.Value;
                            }
                            else if (currentElementName == "td")
                            {
                                if (tdCount == 3)
                                {
                                    book.Author = reader.Value;
                                }
                                else if (tdCount == 4)
                                {
                                    book.Publisher = reader.Value;
                                }
                                else if (tdCount == 5)
                                {
                                    book.PublishYear = reader.Value;
                                }
                                else if (tdCount == 6)
                                {
                                    book.Index = reader.Value;
                                }
                                else if (tdCount == 7)
                                {
                                    book.Total = Int16.Parse(reader.Value);
                                }
                                else if (tdCount == 8)
                                {
                                    book.Available = Int16.Parse(reader.Value);
                                }
                            }
                            break;
                        case XmlNodeType.EndElement:
                            if (reader.Name == "tr")
                            {
                                books.Add(book);
                            }
                            break;
                    }
                }
            }
            return books;
        }
   }
}