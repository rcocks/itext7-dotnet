/*
$Id: 338d2639f7d64c905cbd8d131314a239d8dd4b0f $

This file is part of the iText (R) project.
Copyright (c) 1998-2016 iText Group NV
Authors: Bruno Lowagie, Paulo Soares, et al.

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License version 3
as published by the Free Software Foundation with the addition of the
following permission added to Section 15 as permitted in Section 7(a):
FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
ITEXT GROUP. ITEXT GROUP DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
OF THIRD PARTY RIGHTS

This program is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU Affero General Public License for more details.
You should have received a copy of the GNU Affero General Public License
along with this program; if not, see http://www.gnu.org/licenses or write to
the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
Boston, MA, 02110-1301 USA, or download the license from the following URL:
http://itextpdf.com/terms-of-use/

The interactive user interfaces in modified source and object code versions
of this program must display Appropriate Legal Notices, as required under
Section 5 of the GNU Affero General Public License.

In accordance with Section 7(b) of the GNU Affero General Public License,
a covered work must retain the producer line in every PDF that is created
or manipulated using iText.

You can be released from the requirements of the license by purchasing
a commercial license. Buying such a license is mandatory as soon as you
develop commercial activities involving the iText software without
disclosing the source code of your own applications.
These activities include: offering paid services to customers as an ASP,
serving PDFs on the fly in a web application, shipping iText with a closed
source product.

For more information, please contact iText Software Corp. at this
address: sales@itextpdf.com
*/
using System;
using System.Collections.Generic;
using com.itextpdf.io.font;
using com.itextpdf.kernel.color;
using com.itextpdf.kernel.pdf.action;
using com.itextpdf.kernel.pdf.navigation;

namespace com.itextpdf.kernel.pdf
{
	public class PdfOutline
	{
		public static int FLAG_ITALIC = 1;

		public static int FLAG_BOLD = 2;

		private IList<com.itextpdf.kernel.pdf.PdfOutline> children = new List<com.itextpdf.kernel.pdf.PdfOutline
			>();

		private String title;

		private PdfDictionary content;

		private PdfDestination destination;

		private com.itextpdf.kernel.pdf.PdfOutline parent;

		private PdfDocument pdfDoc;

		public PdfOutline(String title, PdfDictionary content, PdfDocument pdfDocument)
		{
			this.title = title;
			this.content = content;
			this.pdfDoc = pdfDocument;
		}

		public PdfOutline(String title, PdfDictionary content, com.itextpdf.kernel.pdf.PdfOutline
			 parent)
		{
			this.title = title;
			this.content = content;
			this.parent = parent;
			this.pdfDoc = parent.pdfDoc;
			content.MakeIndirect(parent.pdfDoc);
		}

		/// <summary>This constructor creates root outline in the document.</summary>
		/// <param name="doc"/>
		/// <exception cref="com.itextpdf.kernel.PdfException"/>
		protected internal PdfOutline(PdfDocument doc)
		{
			content = new PdfDictionary();
			content.Put(PdfName.Type, PdfName.Outlines);
			this.pdfDoc = doc;
			content.MakeIndirect(doc);
			doc.GetCatalog().AddRootOutline(this);
		}

		public virtual String GetTitle()
		{
			return title;
		}

		public virtual void SetTitle(String title)
		{
			this.title = title;
			this.content.Put(PdfName.Title, new PdfString(title, PdfEncodings.UNICODE_BIG));
		}

		public virtual void SetColor(Color color)
		{
			content.Put(PdfName.C, new PdfArray(color.GetColorValue()));
		}

		public virtual void SetStyle(int style)
		{
			if (style == FLAG_BOLD || style == FLAG_ITALIC)
			{
				content.Put(PdfName.F, new PdfNumber(style));
			}
		}

		public virtual PdfDictionary GetContent()
		{
			return content;
		}

		public virtual IList<com.itextpdf.kernel.pdf.PdfOutline> GetAllChildren()
		{
			return children;
		}

		public virtual com.itextpdf.kernel.pdf.PdfOutline GetParent()
		{
			return parent;
		}

		public virtual PdfDestination GetDestination()
		{
			return destination;
		}

		public virtual void AddDestination(PdfDestination destination)
		{
			SetDestination(destination);
			content.Put(PdfName.Dest, destination.GetPdfObject());
		}

		public virtual void AddAction(PdfAction action)
		{
			content.Put(PdfName.A, action.GetPdfObject());
		}

		/// <summary>
		/// Adds an <CODE>PdfOutline</CODE> as a child to existing <CODE>PdfOutline</CODE>
		/// and put it in the end of the existing <CODE>PdfOutline</CODE> children list
		/// </summary>
		/// <param name="title">an outline title</param>
		/// <returns>a created outline</returns>
		/// <exception cref="com.itextpdf.kernel.PdfException"/>
		public virtual com.itextpdf.kernel.pdf.PdfOutline AddOutline(String title)
		{
			return AddOutline(title, -1);
		}

		/// <summary>
		/// Adds an
		/// <c>PdfOutline</c>
		/// as a child to existing <CODE>PdfOutline</CODE>
		/// and put it to specified position in the existing <CODE>PdfOutline</CODE> children list
		/// </summary>
		/// <param name="title">an outline title</param>
		/// <param name="position">
		/// a position in the current outline child List where a new outline should be added.
		/// If the position equals -1, then the outline will be put in the end of children list.
		/// </param>
		/// <returns>created outline</returns>
		/// <exception cref="com.itextpdf.kernel.PdfException"/>
		public virtual com.itextpdf.kernel.pdf.PdfOutline AddOutline(String title, int position
			)
		{
			if (position == -1)
			{
				position = children.Count;
			}
			PdfDictionary dictionary = new PdfDictionary();
			com.itextpdf.kernel.pdf.PdfOutline outline = new com.itextpdf.kernel.pdf.PdfOutline
				(title, dictionary, this);
			dictionary.Put(PdfName.Title, new PdfString(title, PdfEncodings.UNICODE_BIG));
			dictionary.Put(PdfName.Parent, content);
			if (!children.IsEmpty())
			{
				if (position != 0)
				{
					PdfDictionary prevContent = children[position - 1].GetContent();
					dictionary.Put(PdfName.Prev, prevContent);
					prevContent.Put(PdfName.Next, dictionary);
				}
				if (position != children.Count)
				{
					PdfDictionary nextContent = children[position].GetContent();
					dictionary.Put(PdfName.Next, nextContent);
					nextContent.Put(PdfName.Prev, dictionary);
				}
			}
			if (position == 0)
			{
				content.Put(PdfName.First, dictionary);
			}
			if (position == children.Count)
			{
				content.Put(PdfName.Last, dictionary);
			}
			if (children.Count > 0)
			{
				int count = this.content.GetAsInt(PdfName.Count);
				if (count > 0)
				{
					content.Put(PdfName.Count, new PdfNumber(count++));
				}
				else
				{
					content.Put(PdfName.Count, new PdfNumber(count--));
				}
			}
			else
			{
				this.content.Put(PdfName.Count, new PdfNumber(-1));
			}
			children.Insert(position, outline);
			return outline;
		}

		internal virtual void Clear()
		{
			children.Clear();
		}

		internal virtual void SetDestination(PdfDestination destination)
		{
			this.destination = destination;
		}

		/// <summary>remove this outline from the document.</summary>
		/// <exception cref="com.itextpdf.kernel.PdfException"/>
		internal virtual void RemoveOutline()
		{
			PdfName type = content.GetAsName(PdfName.Type);
			if (type != null && type.Equals(PdfName.Outlines))
			{
				pdfDoc.GetCatalog().Remove(PdfName.Outlines);
				return;
			}
			com.itextpdf.kernel.pdf.PdfOutline parent = this.parent;
			IList<com.itextpdf.kernel.pdf.PdfOutline> children = parent.children;
			children.Remove(this);
			PdfDictionary parentContent = parent.content;
			if (!children.IsEmpty())
			{
				parentContent.Put(PdfName.First, children[0].content);
				parentContent.Put(PdfName.Last, children[children.Count - 1].content);
			}
			else
			{
				parent.RemoveOutline();
				return;
			}
			PdfDictionary next = content.GetAsDictionary(PdfName.Next);
			PdfDictionary prev = content.GetAsDictionary(PdfName.Prev);
			if (prev != null)
			{
				if (next != null)
				{
					prev.Put(PdfName.Next, next);
					next.Put(PdfName.Prev, prev);
				}
				else
				{
					prev.Remove(PdfName.Next);
				}
			}
			else
			{
				if (next != null)
				{
					next.Remove(PdfName.Prev);
				}
			}
		}

		public virtual com.itextpdf.kernel.pdf.PdfOutline AddOutline(com.itextpdf.kernel.pdf.PdfOutline
			 outline)
		{
			com.itextpdf.kernel.pdf.PdfOutline newOutline = AddOutline(outline.GetTitle());
			newOutline.AddDestination(outline.GetDestination());
			IList<com.itextpdf.kernel.pdf.PdfOutline> children = outline.GetAllChildren();
			foreach (com.itextpdf.kernel.pdf.PdfOutline child in children)
			{
				newOutline.AddOutline(child);
			}
			return newOutline;
		}
	}
}