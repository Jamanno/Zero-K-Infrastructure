﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace ZeroKWeb.ForumParser
{
    /// <summary>
    ///     [img]http://url[/img]  or [img=http://url][/img]
    /// </summary>
    public class ImgOpenTag: OpeningArgsTag<ImgCloseTag>
    {
        public override string Match { get; } = "[img";
        public override char MatchTerminator { get; } = ']';

        protected override bool ValidateArgs() => args.Length == 0 || ForumWikiParser.IsValidLink(args.ToString(1, args.Length - 1));


        public override LinkedListNode<Tag> Translate(TranslateContext context, LinkedListNode<Tag> self) {
            var closingTag = self.NextNodeOfType<ImgCloseTag>();

            // get url either from param or from inner literal between tags
            var url = args.Length == 0 ? self.Next.GetOriginalContentUntilNode(closingTag) : args.ToString(1, args.Length - 1);

            if (ForumWikiParser.IsValidLink(url)) context.AppendFormat("<a href=\"{0}\" target=\"_blank\" ><img src=\"{0}\" max-width=\"100%\" height=\"auto\"/></a>", url);

            return closingTag?.Next; // move to after closing img
        }


        public override Tag Create() => new ImgOpenTag();
    }


    public class ImgCloseTag: ScanningTag
    {
        public override string Match { get; } = "[/img]";

        public override LinkedListNode<Tag> Translate(TranslateContext context, LinkedListNode<Tag> self) {
            throw new ApplicationException("This isn't happening"); // should not be called
        }

        public override Tag Create() => new ImgCloseTag();
    }
}