import * as React from "react";

export interface SidebarProps {
  content: React.ReactNode;
  children: React.ReactNode;
}

export interface SidebarItemProps {
  children: React.ReactNode;
  selected?: boolean;
}

export default function Sidebar(props: SidebarProps) {
  return (
    <div className="sidebar">
      <div className="col-auto h-100 overflow-auto">{props.content}</div>
      <div className="col h-100 overflow-auto">{props.children}</div>
    </div>
  );
}

export function SidebarItem(props: SidebarItemProps) {
  return (
    <div className="btn-sidebar" data-selected={props.selected} role="button">
      {props.children}
    </div>
  );
}