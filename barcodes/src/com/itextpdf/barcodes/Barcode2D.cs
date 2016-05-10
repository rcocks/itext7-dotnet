/*
$Id: d9b5a9319e33a1e2af99e9a0ed122daa1d07c80d $

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
using com.itextpdf.kernel.color;
using com.itextpdf.kernel.geom;
using com.itextpdf.kernel.pdf;
using com.itextpdf.kernel.pdf.canvas;
using com.itextpdf.kernel.pdf.xobject;

namespace com.itextpdf.barcodes
{
	public abstract class Barcode2D
	{
		protected internal const float DEFAULT_MODULE_SIZE = 1;

		public Barcode2D()
		{
		}

		/// <summary>
		/// Gets the maximum area that the barcode and the text, if
		/// any, will occupy.
		/// </summary>
		/// <remarks>
		/// Gets the maximum area that the barcode and the text, if
		/// any, will occupy. The lower left corner is always (0, 0).
		/// </remarks>
		/// <returns>the size the barcode occupies.</returns>
		public abstract Rectangle GetBarcodeSize();

		/// <summary>Places the barcode in a <CODE>PdfCanvas</CODE>.</summary>
		/// <remarks>
		/// Places the barcode in a <CODE>PdfCanvas</CODE>. The
		/// barcode is always placed at coordinates (0, 0). Use the
		/// translation matrix to move it elsewhere.
		/// </remarks>
		/// <param name="canvas">the <CODE>PdfCanvas</CODE> where the barcode will be placed</param>
		/// <param name="foreground">the foreground color. It can be <CODE>null</CODE></param>
		/// <returns>the dimensions the barcode occupies</returns>
		public abstract Rectangle PlaceBarcode(PdfCanvas canvas, Color foreground);

		/// <summary>Creates a PdfFormXObject with the barcode.</summary>
		/// <remarks>
		/// Creates a PdfFormXObject with the barcode.
		/// Default foreground color will be used.
		/// </remarks>
		/// <returns>the XObject.</returns>
		public virtual PdfFormXObject CreateFormXObject(PdfDocument document)
		{
			return CreateFormXObject(null, document);
		}

		/// <summary>Creates a PdfFormXObject with the barcode.</summary>
		/// <param name="foreground">the color of the pixels. It can be <CODE>null</CODE></param>
		/// <returns>the XObject.</returns>
		public abstract PdfFormXObject CreateFormXObject(Color foreground, PdfDocument document
			);
	}
}