import {ApplicationRef, ComponentRef, NgModuleRef} from '@angular/core';
import {createNewHosts} from '@angularclass/hmr';

export interface IHmrModule extends NodeModule {
  hot: {
    accept: () => void;
    dispose: (callback: () => void) => void;
  };
}

export const hmrBootstrap = (
  module: NodeModule,
  bootstrap: () => Promise<NgModuleRef<unknown>>
): void => {
  let ngModule: NgModuleRef<unknown>;
  (module as IHmrModule).hot.accept();
  bootstrap().then((mod: NgModuleRef<unknown>) => (ngModule = mod));
  (module as IHmrModule).hot.dispose(() => {
    const appRef: ApplicationRef = ngModule.injector.get(ApplicationRef);
    const elements = appRef.components.map(
      (c: ComponentRef<unknown>) => c.location.nativeElement
    );
    const makeVisible = createNewHosts(elements);
    ngModule.destroy();
    makeVisible();
  });
};
