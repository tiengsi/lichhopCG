export interface MenuOptions {
  scroll?: any;
  submenu?: any;
  accordion?: any;
  dropdown?: any;
}

export interface ScrollTopOptions {
  offset: number;
  speed: number;
}

export interface OffCanvasOptions {
  baseClass: string;
  placement?: string;
  overlay?: boolean;
  closeBy: string;
  toggleBy?: any;
}

export interface ToggleOptions {
  target?: string | any;
  targetState?: string;
  toggleState?: string;
}

export interface HeaderOptions {
  classic?: any;
  offset?: any;
  minimize?: any;
}
