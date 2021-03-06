﻿using System;
using System.Collections.Generic;
using System.Linq;
using StudentNotes.Logic.Consts;
using StudentNotes.Logic.LogicAbstraction;
using StudentNotes.Logic.ViewModels.ManageNotes;
using File = StudentNotes.Repositories.DbModels.File;

namespace StudentNotes.Logic.LogicModels
{
    public class Note : INote
    {
        public int UserId { get; private set; }
        public int NoteId { get; set; }

        public string Name { get; set; }
        public string Category { get; set; }
        public string DestinationPath { get; set; }
        public string Size { get; set; }
        public DateTime UploadDate { get; set; }
        public bool IsShared { get; set; }
        public string Tags { get; set; }
        public byte[] Content { get; set; }
        public NoteAccess AccessThrough { get; set; }
        public NoteTagsViewModel NoteTags { get; set; }

        public Note()
        {
            NoteTags = new NoteTagsViewModel();
        }

        public Note(File file)
        {
            UserId = file.UserId;
            NoteId = file.FileId;
            Name = file.Name;
            Category = file.Category;
            DestinationPath = file.Path;
            Size = file.Size;
            UploadDate = file.UploadDate;
            IsShared = file.IsShared;
            Tags = file.FileTags;
            NoteTags = new NoteTagsViewModel()
            {
                NoteId = file.FileId,
                Tags = file.FileTags
            };
        }

        public Note(int userId)
        {
            UserId = userId;
            NoteTags = new NoteTagsViewModel();
        }

        public List<string> GetTagsList()
        {
            var tags = Tags.Split(new [] {','}, StringSplitOptions.RemoveEmptyEntries);
            return tags.ToList();
        }  

        private List<string> DenormalizeTags(string tagString)
        {
            var splitTagString = tagString.Split(';').ToList();

            splitTagString.RemoveAll(IsEmptyString);

            return splitTagString;
        }

        private bool IsEmptyString(string s)
        {
            return s.Equals("");
        }
    }
}
