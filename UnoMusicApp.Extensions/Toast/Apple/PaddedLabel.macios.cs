using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UnoMusicApp.Extensions.Toast;
public class PaddedLabel : UILabel
{
	/// <summary>
	/// Initialize <see cref="PaddedLabel"/>
	/// </summary>
	public PaddedLabel(NFloat leftPadding, NFloat topPadding, NFloat rightPadding, NFloat bottomPadding)
	{
		LeftPadding = leftPadding;
		TopPadding = topPadding;
		RightPadding = rightPadding;
		BottomPadding = bottomPadding;
	}

	/// <summary>
	/// Left Padding
	/// </summary>
	public NFloat LeftPadding { get; }

	/// <summary>
	/// Top Padding
	/// </summary>
	public NFloat TopPadding { get; }

	/// <summary>
	/// Right Padding
	/// </summary>
	public NFloat RightPadding { get; }

	/// <summary>
	/// Bottom Padding
	/// </summary>
	public NFloat BottomPadding { get; }

	/// <inheritdoc/>
	public override CGSize IntrinsicContentSize => new(
		base.IntrinsicContentSize.Width + LeftPadding + RightPadding,
		base.IntrinsicContentSize.Height + TopPadding + BottomPadding);

	/// <inheritdoc/>
	public override void DrawText(CGRect rect)
	{
		var insets = new UIEdgeInsets(TopPadding, LeftPadding, BottomPadding, RightPadding);
		base.DrawText(insets.InsetRect(rect));
	}
}
