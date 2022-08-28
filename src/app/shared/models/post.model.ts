import { AngularEditorConfig } from '@kolkov/angular-editor';
export class PostModel {
  postId: number;
  title: string;
  filterTitle: string;
  description: string;
  body: string;
  categoryId: number;
  categoryName: string;
  categoryCode: string;
  imagePath: string;
  isActive: boolean;
  createdDate: Date;
  publicId: string;

  constructor() {
    this.title = null;
    this.filterTitle = null;
    this.description = null;
    this.body = null;
    this.categoryId = 0;
    this.categoryName = null;
    this.imagePath = null;
    this.isActive = true;
    this.createdDate = new Date();
    this.publicId = null;
  }
}

export const EditorConfig: AngularEditorConfig = {
  editable: true,
  spellcheck: true,
  height: '15rem',
  minHeight: '5rem',
  placeholder: 'Enter text here...',
  translate: 'no',
  defaultParagraphSeparator: 'p',
  defaultFontName: 'Arial',
  toolbarHiddenButtons: [['bold']],
  customClasses: [
    {
      name: 'quote',
      class: 'quote',
    },
    {
      name: 'redText',
      class: 'redText',
    },
    {
      name: 'titleText',
      class: 'titleText',
      tag: 'h1',
    },
  ],
};
