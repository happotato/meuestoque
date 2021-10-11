import * as React from "react";
import tippy, { Props as TippyProps } from "tippy.js";

export interface PopupProps {
  tippy?: Omit<Partial<TippyProps>, "content">;
  content: React.ReactNode;
  children: JSX.Element;
}

export interface PopupMenuProps extends React.HTMLProps<HTMLUListElement> {
  children: React.ReactNode;
}

export interface PopupMenuItemProps extends React.HTMLProps<HTMLDivElement> {
  children: React.ReactNode;
}

export default function Popup(props: PopupProps) {
  const ref = React.createRef<HTMLDivElement>();

  React.useEffect(() => {
    if (ref.current) {
      const content = ref.current.children[0];
      const child = ref.current.children[1];

      const instance = tippy(child, {
        content,
        animation: "shift-away",
        theme: "light",
        arrow: false,
        offset: [5, 5],
        placement: "bottom",
        ...props.tippy,
      });

      return () => {
        instance.destroy();
      };
    }
  }, []);

  return (
    <div ref={ref}>
      <div className="tooltip-content">{props.content}</div>
      {props.children}
    </div>
  );
}

export function PopupMenu(props: PopupMenuProps) {
  return <ul {...props} className="popup-menu">{props.children}</ul>;
}

export function PopupMenuItem(props: PopupMenuItemProps) {
  return (
    <div className="popup-menu-item">
      <div {...props} role="button">
        {props.children}
      </div>
    </div>
  );
}
